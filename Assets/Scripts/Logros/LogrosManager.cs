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
        int idUsuario = PlayerPrefs.GetInt("user_id", -1);
        if (idUsuario != -1)
            StartCoroutine(CargarLogrosDesideBD(idUsuario));
    }

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
                bool desbloqueado = datoBD.desbloqueado == 1;
                bool reclamado = datoBD.reclamado == 1;

                so.reclamado = reclamado;

                if (desbloqueado)
                    so.progresoActual = so.meta;
                else
                    so.progresoActual = 0;
            }
        }

        Debug.Log("Logros cargados desde BD");
    }

    public void DesbloquearLogro(int idBD)
    {
        LogroSO so = todosLosLogros.Find(l => l.idBD == idBD);
        if (so == null || so.reclamado) return;

        so.progresoActual = so.meta;

        int idUsuario = PlayerPrefs.GetInt("user_id", -1);
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

    public IEnumerator ReclamarLogroEnBD(int idUsuario, int idLogro)
    {
        string json = JsonUtility.ToJson(new DesbloqueoData { id_usuario = idUsuario, id_logro = idLogro });
        UnityWebRequest req = new UnityWebRequest($"{backendURL}/logros/reclamar", "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log($"Logro {idLogro} reclamado en BD");
        else
            Debug.LogError("Error reclamando logro: " + req.error);
    }

    public void IncrementarProgreso(int idBD, int cantidad = 1)
    {
        LogroSO so = todosLosLogros.Find(l => l.idBD == idBD);
        if (so == null || so.reclamado) return;

        so.progresoActual += cantidad;
        if (so.EstaCompletado)
            DesbloquearLogro(idBD);
    }
}

[System.Serializable]
public class LogroData
{
    public int id_logro;
    public string nombre;
    public string descripcion;
    public string icono;
    public int desbloqueado;
    public int reclamado;
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