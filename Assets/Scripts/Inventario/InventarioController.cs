using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class InventarioController : MonoBehaviour
{
    private static readonly Color colorTextoItems = new Color32(0xAD, 0x77, 0x57, 0xFF);

    public Sprite personaje1Sprite;
    public Sprite personaje2Sprite;
    public Sprite personaje3Sprite;

    public Sprite fondo1Sprite;
    public Sprite fondo2Sprite;
    public Sprite fondo3Sprite;
    public Sprite fondo4Sprite;

    public Font fuenteTexto;

    private VisualElement contenedorPersonajes;
    private VisualElement contenedorFondos;
    private Button botonPersonajes;
    private Button botonFondos;
    private Label numMonedas;

    private int id_usuario;
    private int personajeSeleccionado = 0;
    private int fondoSeleccionado     = 0;

    private List<ItemTienda> personajesComprados = new List<ItemTienda>();
    private List<ItemTienda> fondosComprados     = new List<ItemTienda>();

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        fuenteTexto = Resources.Load<Font>("PressStart2P") ?? fuenteTexto;

        contenedorPersonajes = root.Q<VisualElement>("Personajes");
        contenedorFondos     = root.Q<VisualElement>("Fondos");
        botonPersonajes      = root.Q<Button>("BotonPersonajes");
        botonFondos          = root.Q<Button>("BotonFondos");
        numMonedas           = root.Q<Label>("NumMonedas");

        if (botonPersonajes != null) botonPersonajes.pickingMode = PickingMode.Position;
        if (botonFondos != null)     botonFondos.pickingMode     = PickingMode.Position;

        if (contenedorPersonajes != null) contenedorPersonajes.pickingMode = PickingMode.Ignore;
        if (contenedorFondos != null)     contenedorFondos.pickingMode     = PickingMode.Ignore;

        botonPersonajes.clicked += MostrarPersonajes;
        botonFondos.clicked     += MostrarFondos;

        var hud   = root.Q<VisualElement>("HUD");
        var fondo = root.Q<VisualElement>("Fondo");
        if (hud != null)   hud.pickingMode   = PickingMode.Ignore;
        if (fondo != null) fondo.pickingMode = PickingMode.Ignore;

        id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;

        StartCoroutine(CargarMonedas());
        StartCoroutine(CargarInventarioYSeleccion());

        MostrarPersonajes();
    }

    void OnDisable()
    {
        botonPersonajes.clicked -= MostrarPersonajes;
        botonFondos.clicked     -= MostrarFondos;
    }

    IEnumerator CargarMonedas()
    {
        string url = $"https://supernumberland-backend.onrender.com/monedas/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            MonedasResponse res = JsonUtility.FromJson<MonedasResponse>(req.downloadHandler.text);
            if (res.success && numMonedas != null)
                numMonedas.text = res.monedas.ToString();
        }
    }

    IEnumerator CargarInventarioYSeleccion()
    {
        string urlSel = $"https://supernumberland-backend.onrender.com/seleccion/{id_usuario}";
        UnityWebRequest reqSel = UnityWebRequest.Get(urlSel);
        yield return reqSel.SendWebRequest();

        if (reqSel.result == UnityWebRequest.Result.Success)
        {
            SeleccionResponse sel = JsonUtility.FromJson<SeleccionResponse>(reqSel.downloadHandler.text);
            if (sel.success)
            {
                personajeSeleccionado = sel.personaje_seleccionado;
                fondoSeleccionado     = sel.fondo_seleccionado;

                //Actualizar GameManager al cargar
                if (GameManager.instancia != null)
                    GameManager.instancia.personajeSeleccionado = ObtenerIndexPersonaje(personajeSeleccionado);
            }
        }

        string urlTienda = $"https://supernumberland-backend.onrender.com/tienda/{id_usuario}";
        UnityWebRequest reqTienda = UnityWebRequest.Get(urlTienda);
        yield return reqTienda.SendWebRequest();

        if (reqTienda.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al cargar inventario: " + reqTienda.error);
            yield break;
        }

        TiendaResponse res = JsonUtility.FromJson<TiendaResponse>(reqTienda.downloadHandler.text);
        if (!res.success) yield break;

        personajesComprados.Clear();
        fondosComprados.Clear();

        foreach (var item in res.items)
        {
            if (item.id_item <= 3)
            {
                if (item.comprado) personajesComprados.Add(item);
            }
            else
            {
                if (item.comprado || item.id_item == 7)
                    fondosComprados.Add(item);
            }
        }

        LlenarSlots(contenedorPersonajes, personajesComprados, false);
        LlenarSlots(contenedorFondos,     fondosComprados,     true);
    }

    IEnumerator GuardarSeleccion(string tipo, int id_item)
    {
        string url = "https://supernumberland-backend.onrender.com/seleccion";

        SeleccionData data = new SeleccionData
        {
            id_usuario = id_usuario,
            tipo       = tipo,
            id_item    = id_item
        };
        string json = JsonUtility.ToJson(data);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler   = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            if (tipo == "personaje")
            {
                personajeSeleccionado = id_item;

                //Actualizar GameManager para que SpawnerJugador use el personaje correcto
                if (GameManager.instancia != null)
                    GameManager.instancia.personajeSeleccionado = ObtenerIndexPersonaje(id_item);

                LlenarSlots(contenedorPersonajes, personajesComprados, false);
            }
            else
            {
                fondoSeleccionado = id_item;
                LlenarSlots(contenedorFondos, fondosComprados, true);
            }
            Debug.Log($"{tipo} seleccionado: {id_item}");
        }
    }

    //Convierte id_item del backend al index del array de SpawnerJugador
    int ObtenerIndexPersonaje(int id_item)
    {
        switch (id_item)
        {
            case 1: return 1; // Caballero
            case 2: return 2; // Escudero
            case 3: return 3; // Arquera
            default: return 0; // Soldado default
        }
    }

    void LlenarSlots(VisualElement contenedor, List<ItemTienda> comprados, bool esFondo)
    {
        if (contenedor == null) return;

        contenedor.pickingMode = PickingMode.Ignore;

        var slots = contenedor.Children();
        int slotIndex = 0;

        foreach (var slot in slots)
        {
            slot.Clear();
            slot.pickingMode = PickingMode.Ignore;

            if (slotIndex < comprados.Count)
            {
                var item             = comprados[slotIndex];
                Sprite spr           = ObtenerSprite(item.id_item);
                string tipo          = esFondo ? "fondo" : "personaje";
                int seleccionado     = esFondo ? fondoSeleccionado : personajeSeleccionado;
                bool estaSeleccionado = item.id_item == seleccionado;

                var imagen = new VisualElement();
                imagen.style.width           = esFondo ? 240 : 180;
                imagen.style.height          = esFondo ? 140 : 180;
                imagen.style.backgroundImage = new StyleBackground(spr);
                imagen.style.backgroundSize  = new BackgroundSize(BackgroundSizeType.Contain);
                imagen.style.alignSelf       = Align.Center;
                imagen.style.marginTop       = 20;
                imagen.pickingMode           = PickingMode.Ignore;

                var nombre = new Label(item.nombre);
                AplicarFuenteInventario(nombre);
                nombre.style.fontSize                = 25;
                nombre.style.unityFontStyleAndWeight = FontStyle.Normal;
                nombre.style.unityTextAlign          = TextAnchor.MiddleCenter;
                nombre.style.color                   = colorTextoItems;
                nombre.style.marginTop               = 10;
                nombre.pickingMode                   = PickingMode.Ignore;

                var botonSel = new VisualElement();
                botonSel.style.marginTop               = 8;
                botonSel.style.width                   = 240;
                botonSel.style.height                  = 65;
                botonSel.style.backgroundColor         = estaSeleccionado
                    ? new Color(0.1f, 0.6f, 0.1f, 1f)
                    : new Color(0.6f, 0.3f, 0.1f, 1f);
                botonSel.style.borderTopLeftRadius     = 6;
                botonSel.style.borderTopRightRadius    = 6;
                botonSel.style.borderBottomLeftRadius  = 6;
                botonSel.style.borderBottomRightRadius = 6;
                botonSel.style.alignItems              = Align.Center;
                botonSel.style.justifyContent          = Justify.Center;
                botonSel.pickingMode                   = PickingMode.Position;

                var textoBoton = new Label(estaSeleccionado ? "Seleccionado" : "Seleccionar");
                AplicarFuenteInventario(textoBoton);
                textoBoton.style.unityTextAlign = TextAnchor.MiddleCenter;
                textoBoton.style.color          = Color.white;
                textoBoton.style.fontSize       = 18;
                textoBoton.style.whiteSpace     = WhiteSpace.Normal;
                textoBoton.pickingMode          = PickingMode.Ignore;
                botonSel.Add(textoBoton);

                var itemRef = item;
                var tipoRef = tipo;
                botonSel.RegisterCallback<ClickEvent>(evt =>
                {
                    StartCoroutine(GuardarSeleccion(tipoRef, itemRef.id_item));
                });

                slot.style.alignItems     = Align.Center;
                slot.style.justifyContent = Justify.Center;
                slot.style.flexDirection  = FlexDirection.Column;

                slot.Add(imagen);
                slot.Add(nombre);
                slot.Add(botonSel);
            }

            slotIndex++;
        }
    }

    void AplicarFuenteInventario(Label label)
    {
        label.AddToClassList("textoInventario");
        label.style.unityFontStyleAndWeight = FontStyle.Normal;
        if (fuenteTexto != null) label.style.unityFont = fuenteTexto;
    }

    Sprite ObtenerSprite(int id_item)
    {
        switch (id_item)
        {
            case 1: return personaje1Sprite;
            case 2: return personaje2Sprite;
            case 3: return personaje3Sprite;
            case 4: return fondo1Sprite;
            case 5: return fondo2Sprite;
            case 6: return fondo3Sprite;
            case 7: return fondo4Sprite;
            default: return null;
        }
    }

    void MostrarPersonajes()
    {
        contenedorPersonajes.style.display = DisplayStyle.Flex;
        contenedorFondos.style.display     = DisplayStyle.None;
    }

    void MostrarFondos()
    {
        contenedorPersonajes.style.display = DisplayStyle.None;
        contenedorFondos.style.display     = DisplayStyle.Flex;
    }
}

[System.Serializable]
public class SeleccionResponse
{
    public bool success;
    public int  personaje_seleccionado;
    public int  fondo_seleccionado;
}

[System.Serializable]
public class SeleccionData
{
    public int    id_usuario;
    public string tipo;
    public int    id_item;
}