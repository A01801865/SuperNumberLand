using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class UIVidasToolkit : MonoBehaviour
{
    private VisualElement vida1, vida2, vida3;
    private VisualElement pantallaPerder, pantallaGanar;
    private VisualElement estrella1, estrella2, estrella3;
    private VisualElement fondoPerder;

    void Start()
    {
        //Sincronizar tipoActual desde PlayerPrefs al inicio de cada mapa
        if (GameManagerProgreso.Instance != null)
        {
            string tipoGuardado = PlayerPrefs.GetString("tipo_nivel", "suma").ToLower();
            switch (tipoGuardado)
            {
                case "resta":
                    GameManagerProgreso.Instance.SetTipoNivel(GameManagerProgreso.TipoNivel.Resta);
                    break;
                case "multiplicacion":
                    GameManagerProgreso.Instance.SetTipoNivel(GameManagerProgreso.TipoNivel.Multiplicacion);
                    break;
                case "division":
                    GameManagerProgreso.Instance.SetTipoNivel(GameManagerProgreso.TipoNivel.Division);
                    break;
                default:
                    GameManagerProgreso.Instance.SetTipoNivel(GameManagerProgreso.TipoNivel.Suma);
                    break;
            }
            Debug.Log("[UIVidasToolkit] tipoActual sincronizado: " + GameManagerProgreso.Instance.tipoActual);
        }

        var root = GetComponent<UIDocument>().rootVisualElement;

        //Vidas
        vida1 = root.Q<VisualElement>("Vida_1");
        vida2 = root.Q<VisualElement>("Vida_2");
        vida3 = root.Q<VisualElement>("Vida_3");

        //Pantallas
        pantallaPerder = root.Q<VisualElement>("Perder");
        pantallaGanar = root.Q<VisualElement>("Ganar");

        //Estrellas
        estrella1 = root.Q<VisualElement>("Estrella_1");
        estrella2 = root.Q<VisualElement>("Estrella_2");
        estrella3 = root.Q<VisualElement>("Estrella_3");

        //Fondo perder, solo suscribe Reintentar, NO BotonVolver (lo maneja RVolverANiveles/MVolverANiveles/DVolverANiveles)
        fondoPerder = root.Q<VisualElement>("FondoPerder");

        if (fondoPerder != null)
        {
            Button botonReintentar = fondoPerder.Q<Button>("BotonReintentar");
            if (botonReintentar != null)
                botonReintentar.clicked += Reintentar;
        }
    }

  
    //VIDAS
    public void ActualizarVidas(int vidas)
    {
        if (vida1 != null)
            vida1.style.display = vidas >= 1 ? DisplayStyle.Flex : DisplayStyle.None;

        if (vida2 != null)
            vida2.style.display = vidas >= 2 ? DisplayStyle.Flex : DisplayStyle.None;

        if (vida3 != null)
            vida3.style.display = vidas >= 3 ? DisplayStyle.Flex : DisplayStyle.None;
    }

    //PERDER
    public void MostrarPantallaPerder()
    {
        Debug.Log("MOSTRANDO PANTALLA DE PERDER");

        if (pantallaPerder != null)
            pantallaPerder.style.display = DisplayStyle.Flex;

        Time.timeScale = 0f;
    }

    //GANAR

    public void MostrarPantallaGanar(int estrellas)
    {
        Debug.Log("MOSTRANDO PANTALLA DE GANAR");

        if (pantallaGanar != null)
            pantallaGanar.style.display = DisplayStyle.Flex;

        if (estrella1 != null)
            estrella1.style.display = estrellas >= 1 ? DisplayStyle.Flex : DisplayStyle.None;
        if (estrella2 != null)
            estrella2.style.display = estrellas >= 2 ? DisplayStyle.Flex : DisplayStyle.None;
        if (estrella3 != null)
            estrella3.style.display = estrellas >= 3 ? DisplayStyle.Flex : DisplayStyle.None;

        if (GameManagerProgreso.Instance != null && GameManagerProgreso.Instance.vidasPerdidas == 0)
            LogrosManager.Instance?.DesbloquearLogro(11);

        string escena = SceneManager.GetActiveScene().name.ToLower();
        string tipo = "";

        if (escena.Contains("rnivel"))
            tipo = "resta";
        else if (escena.Contains("mnivel"))
            tipo = "multiplicacion";
        else if (escena.Contains("dnivel"))
            tipo = "division";
        else
            tipo = "suma";

        int idNivel = PlayerPrefs.GetInt("nivel_seleccionado", 0);
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;

        if (tipo != "" && idNivel > 0)
            StartCoroutine(GuardarProgreso(id_usuario, idNivel, tipo, estrellas));

        if (MonedaManager.instance != null)
            MonedaManager.instance.EnviarMonedasAlBackend(id_usuario);
    }

    public void MostrarPantallaGanar()
    {
        int estrellas = 3;
        if (GameManagerProgreso.Instance != null)
            estrellas = GameManagerProgreso.Instance.vidasActuales;
        MostrarPantallaGanar(estrellas);
    }

    //REINTENTAR
    private void Reintentar()
    {
        Time.timeScale = 1f;

        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = 3;
            GameManagerProgreso.Instance.nivelActual = 1;
            GameManagerProgreso.Instance.vidasPerdidas = 0;

            switch (GameManagerProgreso.Instance.tipoActual)
            {
                case GameManagerProgreso.TipoNivel.Resta:
                    SceneManager.LoadScene("RNivel1");
                    break;
                case GameManagerProgreso.TipoNivel.Multiplicacion:
                    SceneManager.LoadScene("MNivel1");
                    break;
                case GameManagerProgreso.TipoNivel.Division:
                    SceneManager.LoadScene("DNivel1");
                    break;
                default:
                    SceneManager.LoadScene("Nivel1");
                    break;
            }
        }
        else
        {
            string escena = SceneManager.GetActiveScene().name.ToLower();
            if (escena.Contains("rnivel"))
                SceneManager.LoadScene("RNivel1");
            else if (escena.Contains("mnivel"))
                SceneManager.LoadScene("MNivel1");
            else if (escena.Contains("dnivel"))
                SceneManager.LoadScene("DNivel1");
            else
                SceneManager.LoadScene("Nivel1");
        }
    }

    //BACKEND

    IEnumerator GuardarProgreso(int id_usuario, int id_nivel, string tipo, int estrellas)
    {
        string url = "https://supernumberland-backend.onrender.com/progreso/guardar";

        string json = JsonUtility.ToJson(new ProgresoData
        {
            id_usuario = id_usuario,
            id_nivel = id_nivel,
            tipo = tipo,
            estrellas = estrellas
        });

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log("Progreso guardado correctamente");
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