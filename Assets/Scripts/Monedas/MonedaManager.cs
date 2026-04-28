using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MonedaManager : MonoBehaviour
{
    public static MonedaManager instance;

    public int totalMonedas = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Sumar(int cantidad = 1)
    {
        totalMonedas += cantidad;
    }

    public void EnviarMonedasAlBackend(int id_usuario)
    {
        if (totalMonedas > 0)
            StartCoroutine(EnviarMonedas(id_usuario, totalMonedas));
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
            {
                Debug.Log($"✅ Monedas guardadas. Total en BD: {res.monedas}");

                // Logro: Millonario (id_logro = 10) — se verifica con el total real de la BD
                if (res.monedas >= 500)
                    LogrosManager.Instance?.DesbloquearLogro(10);
            }
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