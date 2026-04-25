using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class TiendaController : MonoBehaviour
{
    // SPRITES
    public Sprite personaje1Sprite;
    public Sprite personaje2Sprite;
    public Sprite personaje3Sprite;

    public Sprite fondo1Sprite;
    public Sprite fondo2Sprite;
    public Sprite fondo3Sprite;

    public Sprite monedaSprite;
    public Font   fuenteTexto;

    // UI
    private VisualElement contenedorPersonajes;
    private VisualElement contenedorFondos;
    private Button botonPersonajes;
    private Button botonFondos;
    private Label numMonedas;

    // DATOS
    private int monedas    = 0;
    private int id_usuario = 0;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        contenedorPersonajes = root.Q<VisualElement>("Personajes");
        contenedorFondos     = root.Q<VisualElement>("Fondos");
        botonPersonajes      = root.Q<Button>("BotonPersonajes");
        botonFondos          = root.Q<Button>("BotonFondos");
        numMonedas           = root.Q<Label>("NumMonedas");

        botonPersonajes.clicked += MostrarPersonajes;
        botonFondos.clicked     += MostrarFondos;

        id_usuario = PlayerPrefs.GetInt("user_id", 0);

        // TEMPORAL PARA PRUEBAS
        if (id_usuario == 0)
        {
            id_usuario = 6;
            Debug.Log("⚠️ Usando id_usuario de prueba: " + id_usuario);
        }

        StartCoroutine(CargarMonedas());
        StartCoroutine(CargarTienda());

        MostrarPersonajes();

        var hud = root.Q<VisualElement>("HUD");
        if (hud != null) hud.pickingMode = PickingMode.Ignore;
    }

    void OnDisable()
    {
        botonPersonajes.clicked -= MostrarPersonajes;
        botonFondos.clicked     -= MostrarFondos;
    }

    // ─── BACKEND ────────────────────────────────────────────────

    IEnumerator CargarMonedas()
    {
        string url = $"https://supernumberland-backend.onrender.com/monedas/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            MonedasResponse res = JsonUtility.FromJson<MonedasResponse>(req.downloadHandler.text);
            if (res.success)
            {
                monedas = res.monedas;
                if (numMonedas != null) numMonedas.text = monedas.ToString();
            }
        }
    }

    IEnumerator CargarTienda()
    {
        string url = $"https://supernumberland-backend.onrender.com/tienda/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error tienda: " + req.error);
            yield break;
        }

        Debug.Log("Respuesta tienda: " + req.downloadHandler.text);

        TiendaResponse res = JsonUtility.FromJson<TiendaResponse>(req.downloadHandler.text);

        Debug.Log("Success: " + res.success);
        Debug.Log("Items count: " + (res.items != null ? res.items.Count.ToString() : "NULL"));

        if (!res.success) yield break;

        foreach (var item in res.items)
        {
            Debug.Log($"Item: {item.id_item} - {item.nombre} - comprado: {item.comprado}");

            string nombreSlot = ObtenerNombreSlot(item.id_item);
            Debug.Log("Slot buscado: " + nombreSlot);

            if (string.IsNullOrEmpty(nombreSlot)) continue;

            var root = GetComponent<UIDocument>().rootVisualElement;
            var slot = root.Q<VisualElement>(nombreSlot);

            Debug.Log("Slot encontrado: " + (slot != null ? "SI" : "NO"));

            if (slot == null) continue;

            LlenarSlot(slot, item);
        }
    }

    IEnumerator ComprarRequest(ItemTienda item, VisualElement slot)
    {
        string url = "https://supernumberland-backend.onrender.com/comprar";

        CompraData data = new CompraData { id_usuario = id_usuario, id_item = item.id_item };
        string json = JsonUtility.ToJson(data);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler   = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            CompraResponse res = JsonUtility.FromJson<CompraResponse>(req.downloadHandler.text);
            if (res.success)
            {
                monedas = res.monedas_restantes;
                if (numMonedas != null) numMonedas.text = monedas.ToString();
                item.comprado = true;
                LlenarSlot(slot, item);
                Debug.Log("✅ Compra exitosa: " + item.nombre);
            }
            else
            {
                Debug.Log("❌ " + res.message);
            }
        }
    }

    // ─── UI ─────────────────────────────────────────────────────

    void LlenarSlot(VisualElement slot, ItemTienda item)
    {
        slot.Clear();
        slot.pickingMode      = PickingMode.Position;
        slot.style.flexDirection  = FlexDirection.Column;
        slot.style.alignItems     = Align.Center;
        slot.style.justifyContent = Justify.Center;

        // Imagen
        var imagen = new VisualElement();
        imagen.style.width           = 180;
        imagen.style.height          = 180;
        imagen.style.backgroundImage = new StyleBackground(ObtenerSprite(item.id_item));
        imagen.style.backgroundSize  = new BackgroundSize(BackgroundSizeType.Contain);
        imagen.style.alignSelf       = Align.Center;
        imagen.pickingMode           = PickingMode.Ignore;

        // Nombre
        var nombre = new Label(item.nombre);
        nombre.style.fontSize                = 22;
        nombre.style.unityFontStyleAndWeight = FontStyle.Bold;
        nombre.style.unityTextAlign          = TextAnchor.MiddleCenter;
        nombre.style.color                   = Color.white;
        nombre.pickingMode                   = PickingMode.Ignore;
        if (fuenteTexto != null) nombre.style.unityFont = fuenteTexto;

        // Precio con icono
        var contPrecio = new VisualElement();
        contPrecio.style.flexDirection  = FlexDirection.Row;
        contPrecio.style.alignItems     = Align.Center;
        contPrecio.style.justifyContent = Justify.Center;
        contPrecio.style.marginTop      = 5;
        contPrecio.pickingMode          = PickingMode.Ignore;

        if (monedaSprite != null)
        {
            var icono = new VisualElement();
            icono.style.width           = 35;
            icono.style.height          = 35;
            icono.style.backgroundImage = new StyleBackground(monedaSprite);
            icono.style.backgroundSize  = new BackgroundSize(BackgroundSizeType.Contain);
            icono.pickingMode           = PickingMode.Ignore;
            contPrecio.Add(icono);
        }

        var precio = new Label(item.precio.ToString());
        precio.style.fontSize = 22;
        precio.style.color    = Color.white;
        precio.pickingMode    = PickingMode.Ignore;
        if (fuenteTexto != null) precio.style.unityFont = fuenteTexto;
        contPrecio.Add(precio);

        // Botón como VisualElement
        var boton = new VisualElement();
        boton.style.marginTop                = 8;
        boton.style.width                    = 180;
        boton.style.height                   = 55;
        boton.style.backgroundColor          = item.comprado 
            ? new Color(0.4f, 0.4f, 0.4f, 1f) 
            : new Color(0.6f, 0.3f, 0.1f, 1f);
        boton.style.borderTopLeftRadius      = 6;
        boton.style.borderTopRightRadius     = 6;
        boton.style.borderBottomLeftRadius   = 6;
        boton.style.borderBottomRightRadius  = 6;
        boton.style.alignItems               = Align.Center;
        boton.style.justifyContent           = Justify.Center;
        boton.pickingMode                    = PickingMode.Position;

        var textoBoton = new Label(item.comprado ? "Comprado" : "Comprar");
        textoBoton.style.unityTextAlign = TextAnchor.MiddleCenter;
        textoBoton.style.color          = Color.white;
        textoBoton.style.fontSize       = 20;
        textoBoton.pickingMode          = PickingMode.Ignore;
        if (fuenteTexto != null) textoBoton.style.unityFont = fuenteTexto;
        boton.Add(textoBoton);

        if (!item.comprado)
        {
            var itemRef = item;
            var slotRef = slot;
            boton.RegisterCallback<ClickEvent>(evt => {
                Debug.Log("Click en: " + itemRef.nombre);
                StartCoroutine(ComprarRequest(itemRef, slotRef));
            });
        }

        slot.Add(imagen);
        slot.Add(nombre);
        slot.Add(contPrecio);
        slot.Add(boton);
    }

    string ObtenerNombreSlot(int id_item)
    {
        switch (id_item)
        {
            case 1: return "Personaje1";
            case 2: return "Personaje2";
            case 3: return "Personaje3";
            case 4: return "Fondo1";
            case 5: return "Fondo2";
            case 6: return "Fondo3";
            default: return null;
        }
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