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

        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel1");
    }

    private void VolverMenu()
    {
        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = 3;
            GameManagerProgreso.Instance.nivelActual   = 1;
            GameManagerProgreso.Instance.vidasPerdidas = 0;
        }

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Niveles");
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
        Debug.Log("MOSTRANDO PANTALLA DE GANAR");

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

        // Enviar monedas al backend
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;

        if (MonedaManager.instance != null && MonedaManager.instance.totalMonedas > 0)
            StartCoroutine(EnviarMonedas(id_usuario, MonedaManager.instance.totalMonedas));
    }

    IEnumerator EnviarMonedas(int id_usuario, int cantidad)
    {
        Debug.Log($"Enviando {cantidad} monedas del usuario {id_usuario}");

        string url  = "https://supernumberland-backend.onrender.com/sumar-monedas";
        string json = JsonUtility.ToJson(new SumarMonedasData
        {
            id_usuario = id_usuario,
            cantidad   = cantidad
        });

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler   = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        Debug.Log("Respuesta: " + req.downloadHandler.text);

        if (req.result == UnityWebRequest.Result.Success)
        {
            SumarMonedasResponse res = JsonUtility.FromJson<SumarMonedasResponse>(req.downloadHandler.text);
            if (res.success)
                Debug.Log($"✅ Monedas guardadas. Total en BD: {res.monedas}");
        }
        else
        {
            Debug.LogError("Error al enviar monedas: " + req.error);
        }
    }
}

[System.Serializable]
public class SumarMonedasData
{
    public int id_usuario;
    public int cantidad;
}

[System.Serializable]
public class SumarMonedasResponse
{
    public bool success;
    public int  monedas;
}