using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class UIVidasToolkit : MonoBehaviour
{
    private VisualElement vida1;
    private VisualElement vida2;
    private VisualElement vida3;
    private VisualElement pantallaPerder;
    private VisualElement pantallaGanar;
    private VisualElement estrella1;
    private VisualElement estrella2;
    private VisualElement estrella3;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        vida1 = root.Q<VisualElement>("Vida_1");
        vida2 = root.Q<VisualElement>("Vida_2");
        vida3 = root.Q<VisualElement>("Vida_3");

        pantallaPerder = root.Q<VisualElement>("Perder");
        pantallaGanar  = root.Q<VisualElement>("Ganar");

        estrella1 = root.Q<VisualElement>("Estrella_1");
        estrella2 = root.Q<VisualElement>("Estrella_2");
        estrella3 = root.Q<VisualElement>("Estrella_3");

        var fondoPerder = root.Q<VisualElement>("FondoPerder");
        if (fondoPerder != null)
        {
            Button botonReintentar = fondoPerder.Q<Button>("BotonReintentar");
            if (botonReintentar != null)
                botonReintentar.clicked += Reintentar;

            Button botonVolverPerder = fondoPerder.Q<Button>("BotonVolver");
            if (botonVolverPerder != null)
                botonVolverPerder.clicked += VolverMenu;
        }

        if (pantallaPerder != null)
            pantallaPerder.style.display = DisplayStyle.None;

        if (pantallaGanar != null)
            pantallaGanar.style.display = DisplayStyle.None;

        if (GameManagerProgreso.Instance != null)
            ActualizarVidas(GameManagerProgreso.Instance.vidasActuales);
    }

    private void Reintentar()
    {
        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = 3;
            GameManagerProgreso.Instance.nivelActual   = 1;
            GameManagerProgreso.Instance.vidasPerdidas = 0;
        }

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        ObjetoRespuesta.ResetearRacha();

        Time.timeScale = 1f;

        string tipo = PlayerPrefs.GetString("tipo_nivel", "suma");
        Debug.Log("Reintentar tipo: " + tipo);
        switch (tipo)
        {
            case "resta":          SceneManager.LoadScene("RNivel1");  break;
            case "multiplicacion": SceneManager.LoadScene("MNivel1");  break;
            case "division":       SceneManager.LoadScene("DNivel1");  break;
            default:               SceneManager.LoadScene("Nivel1");   break;
        }
    }

    private void VolverMenu()
    {
        Debug.Log("VolverMenu tipo: " + PlayerPrefs.GetString("tipo_nivel", "suma"));

        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = 3;
            GameManagerProgreso.Instance.nivelActual   = 1;
            GameManagerProgreso.Instance.vidasPerdidas = 0;
        }

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        ObjetoRespuesta.ResetearRacha();

        Time.timeScale = 1f;

        string tipo = PlayerPrefs.GetString("tipo_nivel", "suma");
        switch (tipo)
        {
            case "resta":          SceneManager.LoadScene("NivelesResta"); break;
            case "multiplicacion": SceneManager.LoadScene("NivelesMulti"); break;
            case "division":       SceneManager.LoadScene("NivelesDivi");  break;
            default:               SceneManager.LoadScene("Niveles");      break;
        }
    }

    public void ActualizarVidas(int vidas)
    {
        if (vida1 != null)
            vida1.style.display = vidas >= 1 ? DisplayStyle.Flex : DisplayStyle.None;
        if (vida2 != null)
            vida2.style.display = vidas >= 2 ? DisplayStyle.Flex : DisplayStyle.None;
        if (vida3 != null)
            vida3.style.display = vidas >= 3 ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void MostrarPantallaPerder()
    {
        Debug.Log("MOSTRANDO PANTALLA DE PERDER");
        if (pantallaPerder != null)
            pantallaPerder.style.display = DisplayStyle.Flex;
        else
            Debug.LogError("No se encontró Perder");
    }

    public void MostrarPantallaGanar()
    {
        Debug.Log("¡GANASTE!");
        Debug.Log("Escena actual: " + SceneManager.GetActiveScene().name);

        if (pantallaGanar != null)
            pantallaGanar.style.display = DisplayStyle.Flex;
        else
            Debug.LogError("No se encontró Ganar");

        int estrellas = GameManagerProgreso.Instance != null
            ? GameManagerProgreso.Instance.CalcularEstrellas()
            : 1;

        if (estrella1 != null)
            estrella1.style.display = estrellas >= 1 ? DisplayStyle.Flex : DisplayStyle.None;
        if (estrella3 != null)
            estrella3.style.display = estrellas >= 2 ? DisplayStyle.Flex : DisplayStyle.None;
        if (estrella2 != null)
            estrella2.style.display = estrellas >= 3 ? DisplayStyle.Flex : DisplayStyle.None;

        if (GameManagerProgreso.Instance != null && GameManagerProgreso.Instance.vidasPerdidas == 0)
            LogrosManager.Instance?.DesbloquearLogro(11);

        string escena = SceneManager.GetActiveScene().name.ToLower();
        string tipo = "";

        if (escena == "nivel1" || escena.Contains("mapa"))
        {
            LogrosManager.Instance?.DesbloquearLogro(7);
            tipo = "suma";
        }
        else if (escena.Contains("rnivel"))
        {
            LogrosManager.Instance?.DesbloquearLogro(5);
            tipo = "resta";
        }
        else if (escena.Contains("mnivel"))
        {
            LogrosManager.Instance?.DesbloquearLogro(6);
            tipo = "multiplicacion";
        }
        else if (escena.Contains("dnivel"))
        {
            tipo = "division";
        }

        int idNivel = PlayerPrefs.GetInt("nivel_seleccionado", 0);
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;

        if (tipo != "" && idNivel > 0)
            StartCoroutine(GuardarProgreso(id_usuario, idNivel, tipo, estrellas));

        if (MonedaManager.instance != null)
            MonedaManager.instance.EnviarMonedasAlBackend(id_usuario);
    }

    IEnumerator GuardarProgreso(int id_usuario, int id_nivel, string tipo, int estrellas)
    {
        string url = "https://supernumberland-backend.onrender.com/progreso/guardar";
        string json = JsonUtility.ToJson(new ProgresoData
        {
            id_usuario = id_usuario,
            id_nivel   = id_nivel,
            tipo       = tipo,
            estrellas  = estrellas
        });

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler   = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log($"✅ Progreso guardado: {tipo} nivel {id_nivel} - {estrellas} estrellas");
        else
            Debug.LogError("Error guardando progreso: " + req.error);
    }
}

[System.Serializable]
public class ProgresoData
{
    public int id_usuario;
    public int id_nivel;
    public string tipo;
    public int estrellas;
}