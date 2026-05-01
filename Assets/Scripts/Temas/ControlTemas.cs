using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class ControlTemas : MonoBehaviour
{
    private Button suma;
    private Button resta;
    private Button multiplicacion;
    private Button division;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        suma = root.Q<Button>("suma");
        resta = root.Q<Button>("resta");
        multiplicacion = root.Q<Button>("multiplicacion");
        division = root.Q<Button>("division");

        // Primero bloquear todo
        suma.SetEnabled(true);
        resta.SetEnabled(true);
        multiplicacion.SetEnabled(false);
        multiplicacion.style.opacity = 0.5f;
        division.SetEnabled(false);
        division.style.opacity = 0.5f;

        int edad = PlayerPrefs.GetInt("edad");
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;

        Debug.Log("Edad jugador: " + edad);

        // Desbloquear por edad
        ConfigurarTemasPorEdad(edad);

        // Desbloquear por rendimiento
        StartCoroutine(ConfigurarTemasPorRendimiento(id_usuario));

        ConfigurarBotones();
    }

    void ConfigurarTemasPorEdad(int edad)
    {
        if (edad >= 8)
        {
            multiplicacion.SetEnabled(true);
            multiplicacion.style.opacity = 1f;
        }

        if (edad >= 10)
        {
            division.SetEnabled(true);
            division.style.opacity = 1f;
        }
    }

    IEnumerator ConfigurarTemasPorRendimiento(int id_usuario)
    {
        string url = $"https://supernumberland-backend.onrender.com/temas-desbloqueados/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error consultando temas desbloqueados: " + req.error);
            yield break;
        }

        TemasDesbloqueadosResponse res = JsonUtility.FromJson<TemasDesbloqueadosResponse>(req.downloadHandler.text);
        if (!res.success) yield break;

        if (res.desbloquear_multiplicacion)
        {
            multiplicacion.SetEnabled(true);
            multiplicacion.style.opacity = 1f;
            Debug.Log("Multiplicación desbloqueada por rendimiento");
        }

        if (res.desbloquear_division)
        {
            division.SetEnabled(true);
            division.style.opacity = 1f;
            Debug.Log("División desbloqueada por rendimiento");
        }
    }

    void ConfigurarBotones()
    {
        suma.clicked           += () => CargarNivel("Niveles",      "suma",           GameManagerProgreso.TipoNivel.Suma);
        resta.clicked          += () => CargarNivel("NivelesResta", "resta",          GameManagerProgreso.TipoNivel.Resta);
        multiplicacion.clicked += () => CargarNivel("NivelesMulti", "multiplicacion", GameManagerProgreso.TipoNivel.Multiplicacion);
        division.clicked       += () => CargarNivel("NivelesDivi",  "division",       GameManagerProgreso.TipoNivel.Division);
    }

    void CargarNivel(string escena, string tipo, GameManagerProgreso.TipoNivel tipoNivel)
    {
        Debug.Log("Cargando: " + escena + " tipo: " + tipo);
        PlayerPrefs.SetString("tipo_nivel", tipo);
        PlayerPrefs.Save();

        if (GameManagerProgreso.Instance != null)
            GameManagerProgreso.Instance.SetTipoNivel(tipoNivel);

        SceneManager.LoadScene(escena);
    }
}

[System.Serializable]
public class TemasDesbloqueadosResponse
{
    public bool success;
    public bool desbloquear_multiplicacion;
    public bool desbloquear_division;
}