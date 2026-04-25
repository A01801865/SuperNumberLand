using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class PerfilController : MonoBehaviour
{
    void Start()
    {
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;

        StartCoroutine(CargarPerfil(id_usuario));
    }

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

        var root = GetComponent<UIDocument>().rootVisualElement;

        var apodo    = root.Q<TextField>("ApodoJugador");
        var nombre   = root.Q<TextField>("NombreJugador");
        var alcaldia = root.Q<TextField>("AlcaldiaJugador");
        var edad     = root.Q<TextField>("EdadJugador");
        var genero   = root.Q<TextField>("GeneroJugador");
        var actividad = root.Q<TextField>("ActividadJugador");

        if (apodo != null)    apodo.value    = res.nombre_usuario;
        if (nombre != null)   nombre.value   = res.nombre_completo;
        if (alcaldia != null) alcaldia.value = res.alcaldia;
        if (edad != null)     edad.value     = res.edad.ToString();
        if (genero != null)   genero.value   = res.genero;
        if (actividad != null) actividad.value = res.actividad ?? "Ninguna";
    }
}

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