using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class NivelesController : MonoBehaviour
{
    private string[] nombresBotones;

    void OnEnable()
    {
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;

        string tipo = PlayerPrefs.GetString("tipo_nivel", "suma");
        Debug.Log("NivelesController cargando tipo: " + tipo);

        // 🔥 SINCRONIZAR CON GAME MANAGER
        if (GameManagerProgreso.Instance != null)
        {
            switch (tipo)
            {
                case "resta":
                    GameManagerProgreso.Instance.tipoActual = GameManagerProgreso.TipoNivel.Resta;
                    break;
                case "multiplicacion":
                    GameManagerProgreso.Instance.tipoActual = GameManagerProgreso.TipoNivel.Multiplicacion;
                    break;
                case "division":
                    GameManagerProgreso.Instance.tipoActual = GameManagerProgreso.TipoNivel.Division;
                    break;
                default:
                    GameManagerProgreso.Instance.tipoActual = GameManagerProgreso.TipoNivel.Suma;
                    break;
            }
        }

        // 🔘 BOTONES SEGÚN TIPO
        switch (tipo)
        {
            case "resta":
                nombresBotones = new string[] { "RUno", "RDos", "RTres", "RCuatro", "RCinco", "RSeis", "RSiete", "ROcho", "RNueve", "RDiez" };
                break;
            case "multiplicacion":
                nombresBotones = new string[] { "MUno", "MDos", "MTres", "MCuatro", "MCinco", "MSeis", "MSiete", "MOcho", "MNueve", "MDiez" };
                break;
            case "division":
                nombresBotones = new string[] { "DUno", "DDos", "DTres", "DCuatro", "DCinco", "DSeis", "DSiete", "DOcho", "DNueve", "DDiez" };
                break;
            default:
                nombresBotones = new string[] { "Uno", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve", "Diez" };
                break;
        }

        StartCoroutine(CargarEstrellas(id_usuario, tipo));
    }

    IEnumerator CargarEstrellas(int id_usuario, string tipo)
    {
        string url = $"https://supernumberland-backend.onrender.com/estrellas/{id_usuario}/{tipo}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error cargando estrellas: " + req.error);
            yield break;
        }

        EstrellasResponse res = JsonUtility.FromJson<EstrellasResponse>(req.downloadHandler.text);
        if (res == null || !res.success)
        {
            Debug.LogError("Respuesta inválida del servidor");
            yield break;
        }

        Dictionary<int, int> estrellasDict = new Dictionary<int, int>();
        foreach (var n in res.niveles)
            estrellasDict[n.id_nivel] = n.estrellas;

        var root = GetComponent<UIDocument>().rootVisualElement;

        for (int i = 0; i < nombresBotones.Length; i++)
        {
            int numeroNivel = i + 1;
            Button boton = root.Q<Button>(nombresBotones[i]);
            if (boton == null) continue;

            int estrellas = estrellasDict.ContainsKey(numeroNivel) ? estrellasDict[numeroNivel] : 0;

            var e1 = boton.Q<VisualElement>("Estrella_1");
            var e2 = boton.Q<VisualElement>("Estrella_2");
            var e3 = boton.Q<VisualElement>("Estrella_3");

            if (e1 != null) e1.style.opacity = estrellas >= 1 ? 1f : 0.2f;
            if (e2 != null) e2.style.opacity = estrellas >= 2 ? 1f : 0.2f;
            if (e3 != null) e3.style.opacity = estrellas >= 3 ? 1f : 0.2f;
        }

        Debug.Log("✅ Estrellas cargadas correctamente");
    }
}

[System.Serializable]
public class NivelEstrellas
{
    public int id_nivel;
    public int estrellas;
}

[System.Serializable]
public class EstrellasResponse
{
    public bool success;
    public List<NivelEstrellas> niveles;
}