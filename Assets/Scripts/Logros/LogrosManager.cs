using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LogrosManager : MonoBehaviour
{
    public static LogrosManager Instance;

    [Header("Logros ScriptableObjects")]
    public List<LogroSO> todosLosLogros;

    private string backendURL = "https://supernumberland-backend.onrender.com";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int idUsuario = PlayerPrefs.GetInt("id_usuario", -1);
        if (idUsuario != -1)
            StartCoroutine(CargarLogrosDesideBD(idUsuario));
    }

    // Carga el estado de los logros desde la BD y actualiza los SOs
    public IEnumerator CargarLogrosDesideBD(int idUsuario)
    {
        UnityWebRequest req = UnityWebRequest.Get($"{backendURL}/logros/{idUsuario}");
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error cargando logros: " + req.error);
            yield break;
        }

        LogrosResponse resp = JsonUtility.FromJson<LogrosResponse>(req.downloadHandler.text);
        if (!resp.success) yield break;

        foreach (var datoBD in resp.logros)
        {
            LogroSO so = todosLosLogros.Find(l => l.idBD == datoBD.id_logro);
            if (so != null)
            {
                so.reclamado = datoBD.desbloqueado == 1;
                if (so.reclamado)
                    so.progresoActual = so.meta; // marcarlo completo
            }
        }

        Debug.Log("Logros cargados desde BD");
    }

    // Llama esto desde cualquier script para desbloquear un logro
    public void DesbloquearLogro(int idBD)
    {
        LogroSO so = todosLosLogros.Find(l => l.idBD == idBD);
        if (so == null || so.reclamado) return;

        so.progresoActual = so.meta;
        so.reclamado = true;

        int idUsuario = PlayerPrefs.GetInt("id_usuario", -1);
        if (idUsuario != -1)
            StartCoroutine(EnviarDesbloqueoABD(idUsuario, idBD));
    }

    private IEnumerator EnviarDesbloqueoABD(int idUsuario, int idLogro)
    {
        string json = JsonUtility.ToJson(new DesbloqueoData { id_usuario = idUsuario, id_logro = idLogro });
        UnityWebRequest req = new UnityWebRequest($"{backendURL}/logros/desbloquear", "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log($"Logro {idLogro} desbloqueado en BD");
        else
            Debug.LogError("Error desbloqueando logro: " + req.error);
    }

    // Incrementa progreso (para logros con barra como "Contra el Reloj")
    public void IncrementarProgreso(int idBD, int cantidad = 1)
    {
        LogroSO so = todosLosLogros.Find(l => l.idBD == idBD);
        if (so == null || so.reclamado) return;

        so.progresoActual += cantidad;
        if (so.EstaCompletado)
            DesbloquearLogro(idBD);
    }
}

// Clases JSON
[System.Serializable]
public class LogroData
{
    public int id_logro;
    public string nombre;
    public string descripcion;
    public string icono;
    public int desbloqueado;
}

[System.Serializable]
public class LogrosResponse
{
    public bool success;
    public List<LogroData> logros;
}

[System.Serializable]
public class DesbloqueoData
{
    public int id_usuario;
    public int id_logro;
}