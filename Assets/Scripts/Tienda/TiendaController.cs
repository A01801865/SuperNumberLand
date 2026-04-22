using UnityEngine;
using UnityEngine.UIElements;
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
    public Sprite botonSprite;

    // 🔥 FUENTE DESDE INSPECTOR
    public Font fuenteTexto;

    // UI
    private VisualElement contenedorPersonajes;
    private VisualElement contenedorFondos;

    private Button botonPersonajes;
    private Button botonFondos;

    private Label numMonedas;

    // DATOS
    private int monedas = 0;

    private List<ItemTienda> personajes = new List<ItemTienda>();
    private List<ItemTienda> fondos = new List<ItemTienda>();

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        contenedorPersonajes = root.Q<VisualElement>("Personajes");
        contenedorFondos = root.Q<VisualElement>("Fondos");

        botonPersonajes = root.Q<Button>("BotonPersonajes");
        botonFondos = root.Q<Button>("BotonFondos");

        numMonedas = root.Q<Label>("NumMonedas");

        botonPersonajes.clicked += MostrarPersonajes;
        botonFondos.clicked += MostrarFondos;

        ConfigurarContenedor(contenedorPersonajes);
        ConfigurarContenedor(contenedorFondos);

        CargarDatos();
        ActualizarUI();

        MostrarPersonajes();
    }

    void OnDisable()
    {
        botonPersonajes.clicked -= MostrarPersonajes;
        botonFondos.clicked -= MostrarFondos;
    }

    void ConfigurarContenedor(VisualElement contenedor)
    {
        contenedor.style.flexDirection = FlexDirection.Row;
        contenedor.style.flexWrap = Wrap.Wrap;
        contenedor.style.justifyContent = Justify.Center;
    }

    void CargarDatos()
    {
        personajes.Clear();
        fondos.Clear();

        // PERSONAJES
        personajes.Add(new ItemTienda { nombre = "Caballero", precio = 50, imagen = personaje1Sprite });
        personajes.Add(new ItemTienda { nombre = "Escudero", precio = 100, imagen = personaje2Sprite });
        personajes.Add(new ItemTienda { nombre = "Arquera", precio = 150, imagen = personaje3Sprite });

        // FONDOS
        fondos.Add(new ItemTienda { nombre = "Fondo Noche", precio = 80, imagen = fondo1Sprite });
        fondos.Add(new ItemTienda { nombre = "Fondo Árboles", precio = 120, imagen = fondo2Sprite });
        fondos.Add(new ItemTienda { nombre = "Fondo Nubes", precio = 200, imagen = fondo3Sprite });
    }

    void ActualizarUI()
    {
        if (numMonedas != null)
        {
            numMonedas.text = monedas.ToString();
            numMonedas.style.fontSize = 26;
            numMonedas.style.unityFont = fuenteTexto;
        }

        GenerarItems(contenedorPersonajes, personajes);
        GenerarItems(contenedorFondos, fondos);
    }

    void GenerarItems(VisualElement contenedor, List<ItemTienda> lista)
    {
        contenedor.Clear();

        foreach (var item in lista)
        {
            var caja = new VisualElement();

            caja.style.width = 340;
            caja.style.height = 360;
            caja.style.marginLeft = 10;
            caja.style.marginRight = 10;
            caja.style.marginTop = 10;
            caja.style.backgroundColor = new Color(0, 0, 0, 0);

            caja.style.flexDirection = FlexDirection.Column;
            caja.style.alignItems = Align.Center;
            caja.style.justifyContent = Justify.Center;

            // IMAGEN
            var imagen = new VisualElement();
            imagen.style.width = 220;
            imagen.style.height = 220;
            imagen.style.backgroundImage = new StyleBackground(item.imagen);
            imagen.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

            // NOMBRE
            var nombre = new Label(item.nombre);
            nombre.style.fontSize = 26;
            nombre.style.unityFontStyleAndWeight = FontStyle.Bold;
            nombre.style.unityTextAlign = TextAnchor.MiddleCenter;
            nombre.style.unityFont = fuenteTexto;

            // PRECIO
            var contPrecio = new VisualElement();
            contPrecio.style.flexDirection = FlexDirection.Row;
            contPrecio.style.alignItems = Align.Center;
            contPrecio.style.justifyContent = Justify.Center;

            var icono = new VisualElement();
            icono.style.width = 45;
            icono.style.height = 45;
            icono.style.backgroundImage = new StyleBackground(monedaSprite);
            icono.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

            var precio = new Label(item.precio.ToString());
            precio.style.fontSize = 24;
            precio.style.unityFont = fuenteTexto;

            contPrecio.Add(icono);
            contPrecio.Add(precio);

            // BOTÓN
            var boton = new Button(() => Comprar(item))
            {
                text = item.comprado ? "Comprado" : "Comprar"
            };

            boton.style.width = 200;
            boton.style.height = 70;
            boton.style.fontSize = 22;

            boton.style.backgroundImage = new StyleBackground(botonSprite);
            boton.style.unityBackgroundScaleMode = ScaleMode.StretchToFill;

            boton.style.borderBottomWidth = 0;
            boton.style.borderTopWidth = 0;
            boton.style.borderLeftWidth = 0;
            boton.style.borderRightWidth = 0;

            boton.style.unityTextAlign = TextAnchor.MiddleCenter;
            boton.style.color = Color.white;

            boton.style.unityFont = fuenteTexto;

            // AGREGAR
            caja.Add(imagen);
            caja.Add(nombre);
            caja.Add(contPrecio);
            caja.Add(boton);

            contenedor.Add(caja);
        }
    }

    void Comprar(ItemTienda item)
    {
        if (item.comprado) return;

        if (monedas >= item.precio)
        {
            monedas -= item.precio;
            item.comprado = true;
            ActualizarUI();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas");
        }
    }

    void MostrarPersonajes()
    {
        contenedorPersonajes.style.display = DisplayStyle.Flex;
        contenedorFondos.style.display = DisplayStyle.None;
    }

    void MostrarFondos()
    {
        contenedorPersonajes.style.display = DisplayStyle.None;
        contenedorFondos.style.display = DisplayStyle.Flex;
    }

    public void SetMonedasDesdeBD(int monedasBD)
    {
        monedas = monedasBD;
        ActualizarUI();
    }
}
