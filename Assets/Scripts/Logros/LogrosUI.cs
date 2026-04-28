using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class LogrosUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public VisualTreeAsset logroTemplate;
    
    [Header("Datos")]
    public List<LogroSO> listaDeLogros;
    
    private VisualElement contenedor;

    void Start() 
    {
        StartCoroutine(EsperarYActualizar());
    }

    IEnumerator EsperarYActualizar()
    {
        yield return null;

        var uiDocument = GetComponent<UIDocument>();
        
        if (uiDocument == null || uiDocument.rootVisualElement == null)
        {
            Debug.LogWarning("Esperando a que el UIDocument esté listo...");
            yield return new WaitForSeconds(0.1f);
            uiDocument = GetComponent<UIDocument>();
        }

        if (uiDocument != null && uiDocument.rootVisualElement != null)
        {
            var root = uiDocument.rootVisualElement;
            contenedor = root.Q<VisualElement>("contenedor-logros"); 

            if (contenedor != null)
            {
                int idUsuario = PlayerPrefs.GetInt("user_id", 0);
                if (idUsuario == 0) idUsuario = 6;

                if (LogrosManager.Instance != null)
                    yield return StartCoroutine(LogrosManager.Instance.CargarLogrosDesideBD(idUsuario));

                ActualizarInterfaz();
            }
            else
            {
                Debug.LogError("No encontré 'contenedor-logros'. Revisa el nombre en UI Builder.");
            }
        }
    }

    public void ActualizarInterfaz()
    {
        if (contenedor == null) return;
        contenedor.Clear(); 

        foreach (LogroSO logro in listaDeLogros)
        {
            VisualElement fila = logroTemplate.Instantiate();

            fila.Q<Label>("LabelTitulo").text = logro.titulo;
            fila.Q<Label>("LabelMonedas").text = $"+{logro.monedasRecompensa}";

            ProgressBar barra = fila.Q<ProgressBar>("BarraProgreso");
            if (barra != null)
            {
                barra.highValue = logro.meta;
                barra.value = logro.progresoActual;
                barra.title = $"{logro.progresoActual} / {logro.meta}";
            }

            Button btn = fila.Q<Button>("BotonCobrar");
            if (btn != null)
            {
                if (logro.reclamado)
                {
                    btn.text = "Canjeado";
                    btn.SetEnabled(false);
                }
                else if (logro.EstaCompletado)
                {
                    btn.text = "¡Recoger!";
                    btn.SetEnabled(true);
                    btn.clicked += () => {
                        int idUsuario = PlayerPrefs.GetInt("user_id", 0);
                        if (idUsuario == 0) idUsuario = 6;
                        StartCoroutine(ReclamarRecompensa(logro, idUsuario));
                    };
                }
                else
                {
                    btn.text = "Bloqueado";
                    btn.SetEnabled(false);
                }
            }

            contenedor.Add(fila);
        }
    }

    IEnumerator ReclamarRecompensa(LogroSO logro, int idUsuario)
    {
        if (logro.monedasRecompensa > 0)
        {
            string url = "https://supernumberland-backend.onrender.com/sumar-monedas";
            string json = JsonUtility.ToJson(new SumarMonedasData
            {
                id_usuario = idUsuario,
                cantidad   = logro.monedasRecompensa
            });

            UnityWebRequest req = new UnityWebRequest(url, "POST");
            req.uploadHandler   = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
                Debug.Log($"✅ Recompensa de {logro.monedasRecompensa} monedas enviada");
            else
                Debug.LogError("Error al reclamar recompensa: " + req.error);
        }

        logro.reclamado = true;
        ActualizarInterfaz();
    }
}