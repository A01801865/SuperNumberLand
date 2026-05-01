using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class HUDController : MonoBehaviour
{
    // Sprites disponibles para el fondo del juego
    public Sprite fondoDefault;
    public Sprite fondoNoche;
    public Sprite fondoArboles;
    public Sprite fondoNubes;

    // Referencia al renderer del objeto de fondo en escena
    private SpriteRenderer fondoRenderer;

    void Start()
    {
        // Buscar el objeto "back" en la escena y obtener su SpriteRenderer
        GameObject backObj = GameObject.Find("back");
        if (backObj != null)
            fondoRenderer = backObj.GetComponent<SpriteRenderer>();

        // Obtener el UIDocument y su elemento raíz
        var doc = GetComponent<UIDocument>();
        if (doc == null) return;

        var root = doc.rootVisualElement;
        if (root == null) return;

        // Buscar el label que muestra el número de monedas
        var numMonedas = root.Q<Label>("NumMonedas");

        // Buscar el campo de texto del apodo dentro del HUD (o en la raíz si no hay HUD)
        var hud   = root.Q<VisualElement>("HUD");
        var apodo = hud != null ? hud.Q<TextField>("ApodoJugador") : root.Q<TextField>("ApodoJugador");

        // Leer datos del jugador guardados localmente
        string usuario    = PlayerPrefs.GetString("usuario", "");
        int    id_usuario = PlayerPrefs.GetInt("user_id", 0);

        // Mostrar el apodo del jugador si existe
        if (apodo != null && !string.IsNullOrEmpty(usuario))
            apodo.value = usuario;

        // Usar ID de prueba si no hay sesión activa
        if (id_usuario == 0) id_usuario = 6;

        // Iniciar carga de monedas y fondo desde el servidor
        if (numMonedas != null)
            StartCoroutine(CargarMonedas(id_usuario, numMonedas));

        StartCoroutine(CargarFondo(id_usuario));
    }

    // Consulta al servidor cuántas monedas tiene el jugador y las muestra en el HUD
    IEnumerator CargarMonedas(int id_usuario, Label numMonedas)
    {
        string url = $"https://supernumberland-backend.onrender.com/monedas/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            MonedasResponse res = JsonUtility.FromJson<MonedasResponse>(req.downloadHandler.text);
            if (res.success && numMonedas != null)
                numMonedas.text = res.monedas.ToString(); // Actualizar el texto con el valor recibido
        }
    }

    // Consulta al servidor qué fondo tiene seleccionado el jugador y lo aplica en escena
    IEnumerator CargarFondo(int id_usuario)
    {
        string url = $"https://supernumberland-backend.onrender.com/seleccion/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            SeleccionResponse res = JsonUtility.FromJson<SeleccionResponse>(req.downloadHandler.text);
            if (res.success && fondoRenderer != null)
            {
                // Asignar el sprite correspondiente y ajustar su escala a la pantalla
                fondoRenderer.sprite = ObtenerSpriteFondo(res.fondo_seleccionado);
                AjustarEscala();
            }
        }
    }

    // Escala el sprite del fondo para que cubra exactamente el área visible de la cámara
    void AjustarEscala()
    {
        if (fondoRenderer == null || fondoRenderer.sprite == null) return;

        Camera cam = Camera.main;

        // Calcular el tamaño del mundo visible por la cámara
        float alturaWorld  = cam.orthographicSize * 2f;
        float anchoWorld   = alturaWorld * cam.aspect;

        // Calcular el tamaño original del sprite
        float alturaSprite = fondoRenderer.sprite.bounds.size.y;
        float anchoSprite  = fondoRenderer.sprite.bounds.size.x;

        // Calcular la escala necesaria para que el sprite llene la pantalla
        float scaleX = anchoWorld  / anchoSprite;
        float scaleY = alturaWorld / alturaSprite;

        fondoRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }

    // Devuelve el sprite de fondo que corresponde al ítem seleccionado por el jugador
    Sprite ObtenerSpriteFondo(int id_item)
    {
        switch (id_item)
        {
            case 4: return fondoNoche;
            case 5: return fondoArboles;
            case 6: return fondoNubes;
            case 7: return fondoDefault;
            default: return fondoDefault; // Si el ID no es reconocido, usar el fondo por defecto
        }
    }
}