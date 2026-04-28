using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ProgresoUI : MonoBehaviour
{
    public UIDocument uiDocument;

    void OnEnable()
    {
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);
        if (id_usuario == 0) id_usuario = 6;
        StartCoroutine(CargarProgreso(id_usuario));
    }

    IEnumerator CargarProgreso(int id_usuario)
    {
        string url = $"https://supernumberland-backend.onrender.com/progreso/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error cargando progreso: " + req.error);
            yield break;
        }

        ProgresoResponse res = JsonUtility.FromJson<ProgresoResponse>(req.downloadHandler.text);
        if (!res.success) yield break;

        // Parsear niveles por tipo
        int sumas = 0, restas = 0, multiplicacion = 0, division = 0;
        foreach (var nivel in res.niveles)
        {
            if (nivel.tipo == "suma")                sumas = nivel.completados;
            else if (nivel.tipo == "resta")          restas = nivel.completados;
            else if (nivel.tipo == "multiplicacion") multiplicacion = nivel.completados;
            else if (nivel.tipo == "division")       division = nivel.completados;
        }

        int totalNiveles   = sumas + restas + multiplicacion + division;
        int itemsComprados = res.items_comprados;

        var root = uiDocument.rootVisualElement;

        // Barra 1 — Sumas
        SetFila(root, "InfoProgresoUno", "ValorProgresoUno", "BarraUno",
                "Sumas", sumas, 10);

        // Barra 2 — Restas
        SetFila(root, "InfoProgresoDos", "ValorProgresoDos", "BarraDos",
                "Restas", restas, 10);

        // Barra 3 — Multiplicacion
        SetFila(root, "InfoProgresoTres", "ValorProgresoTres", "BarraTres",
                "Multiplicacion", multiplicacion, 10);

        // Barra 4 — Division
        SetFila(root, "InfoProgresoCuatro", "ValorProgresoCuatro", "BarraCuatro",
                "Division", division, 10);

        // Barra 5 — Total niveles
        SetFila(root, "InfoProgresoCinco", "ValorProgresoCinco", "BarraCinco",
                "Total Niveles", totalNiveles, 40);

        // Barra 6 — Maestro Tienda
        SetFila(root, "InfoProgresoSeis", "ValorProgresoSeis", "BarraSeis",
                "Maestro Tienda", itemsComprados, 6);
    }

    void SetFila(VisualElement root, string nombreInfo, string nombreValor, string nombreBarra,
                 string etiqueta, int actual, int total)
    {
        Label info  = root.Q<Label>(nombreInfo);
        Label valor = root.Q<Label>(nombreValor);
        VisualElement barra = root.Q<VisualElement>(nombreBarra);

        if (info == null || valor == null || barra == null)
        {
            Debug.LogError($"No se encontró: {nombreInfo}, {nombreValor} o {nombreBarra}");
            return;
        }

        info.text  = etiqueta;
        valor.text = $"{actual}/{total}";

        float porcentaje = total == 0 ? 0 : (float)actual / total * 100f;
        barra.style.width = Length.Percent(porcentaje);
    }
}

[System.Serializable]
public class NivelProgreso
{
    public string tipo;
    public int completados;
}

[System.Serializable]
public class ProgresoResponse
{
    public bool success;
    public List<NivelProgreso> niveles;
    public int items_comprados;
}