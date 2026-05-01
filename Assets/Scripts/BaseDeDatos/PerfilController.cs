using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class PerfilController : MonoBehaviour
{
    // Sprites asignables desde el Inspector para cada personaje disponible
    [Header("Sprites de personajes")]
    public Sprite personaje0Sprite;
    public Sprite personaje1Sprite;
    public Sprite personaje2Sprite;
    public Sprite personaje3Sprite;

    void Start()
    {
        // Obtener el ID del jugador guardado; usar ID de prueba si no hay sesión activa
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;

        // Cargar datos del perfil y el personaje seleccionado en paralelo
        StartCoroutine(CargarPerfil(id_usuario));
        StartCoroutine(CargarPersonaje(id_usuario));
    }

    // Consulta el perfil del jugador al servidor y rellena los campos de la UI
    IEnumerator CargarPerfil(int id_usuario)
    {
        string url = $"https://supernumberland-backend.onrender.com/perfil/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al cargar perfil: " + req.error);
            yield break;
        }

        PerfilResponse res = JsonUtility.FromJson<PerfilResponse>(req.downloadHandler.text);
        if (!res.success) yield break;

        // Obtener referencias a los campos de texto del perfil en la UI
        var root = GetComponent<UIDocument>().rootVisualElement;

        var apodo     = root.Q<TextField>("ApodoJugador");
        var nombre    = root.Q<TextField>("NombreJugador");
        var alcaldia  = root.Q<TextField>("AlcaldiaJugador");
        var edad      = root.Q<TextField>("EdadJugador");
        var genero    = root.Q<TextField>("GeneroJugador");
        var actividad = root.Q<TextField>("ActividadJugador");

        // Rellenar cada campo con los datos recibidos del servidor
        if (apodo != null)     apodo.value     = res.nombre_usuario;
        if (nombre != null)    nombre.value    = res.nombre_completo;
        if (alcaldia != null)  alcaldia.value  = res.alcaldia;
        if (edad != null)      edad.value      = res.edad.ToString();
        if (genero != null)    genero.value    = res.genero;
        // Mostrar "Ninguna" si el jugador no tiene actividad registrada
        if (actividad != null) actividad.value = string.IsNullOrEmpty(res.actividad) ? "Ninguna" : res.actividad;
    }

    // Consulta qué personaje tiene seleccionado el jugador y lo muestra como foto de perfil
    IEnumerator CargarPersonaje(int id_usuario)
    {
        string url = $"https://supernumberland-backend.onrender.com/seleccion/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al cargar seleccion: " + req.error);
            yield break;
        }

        SeleccionResponse res = JsonUtility.FromJson<SeleccionResponse>(req.downloadHandler.text);
        if (!res.success) yield break;

        var root       = GetComponent<UIDocument>().rootVisualElement;
        var fotoPerfil = root.Q<VisualElement>("FotoPerfil");

        // Aplicar el sprite del personaje como fondo del elemento de foto de perfil
        if (fotoPerfil != null)
        {
            Sprite sprite = ObtenerSpritePersonaje(res.personaje_seleccionado);
            if (sprite != null)
            {
                fotoPerfil.style.backgroundImage = new StyleBackground(sprite);
                fotoPerfil.style.backgroundSize  = new BackgroundSize(BackgroundSizeType.Contain);
                fotoPerfil.style.unityBackgroundImageTintColor = Color.white;
            }
        }
    }

    // Devuelve el sprite que corresponde al ID de personaje seleccionado
    Sprite ObtenerSpritePersonaje(int id_item)
    {
        switch (id_item)
        {
            case 1: return personaje1Sprite;
            case 2: return personaje2Sprite;
            case 3: return personaje3Sprite;
            default: return personaje0Sprite; // Personaje por defecto si el ID no es reconocido
        }
    }
}

// Estructura que mapea la respuesta JSON del endpoint de perfil
[System.Serializable]
public class PerfilResponse
{
    public bool   success;
    public string nombre_usuario;
    public string nombre_completo;
    public int    edad;
    public string genero;
    public string alcaldia;
    public string actividad;
}