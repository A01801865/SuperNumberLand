using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MonedaManager : MonoBehaviour
{
    // Instancia global accesible desde cualquier script
    public static MonedaManager instance;

    public int totalMonedas = 0;

    void Awake()
    {
        //conservar una sola instancia entre escenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject); // Destruir duplicados si ya existe una instancia
    }

    // Suma monedas al contador local (por defecto agrega 1)
    public void Sumar(int cantidad = 1)
    {
        totalMonedas += cantidad;
    }

    // Inicia el envío de monedas al servidor solo si hay monedas acumuladas
    public void EnviarMonedasAlBackend(int id_usuario)
    {
        if (totalMonedas > 0)
            StartCoroutine(EnviarMonedas(id_usuario, totalMonedas));
    }

    // Envía las monedas acumuladas al servidor y verifica logros con el total actualizado
    IEnumerator EnviarMonedas(int id_usuario, int cantidad)
    {
        Debug.Log($"Enviando {cantidad} monedas del usuario {id_usuario}");

        // Construir y enviar la petición POST con las monedas a sumar
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

                // Verificar logro "Millonario" usando el total real devuelto por la BD
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

// Datos que se envían al servidor para sumar monedas
[System.Serializable]
public class SumarMonedasData
{
    public int id_usuario;
    public int cantidad;
}

// Respuesta del servidor tras sumar las monedas
[System.Serializable]
public class SumarMonedasResponse
{
    public bool success;
    public int  monedas; // Total acumulado del jugador en la base de datos
}