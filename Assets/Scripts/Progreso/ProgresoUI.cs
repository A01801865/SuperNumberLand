using UnityEngine;
using UnityEngine.UIElements;

public class ProgresoUI : MonoBehaviour
{
    public UIDocument uiDocument;

    void Start()
    {
        var root = uiDocument.rootVisualElement;

        // ejemplos de datos (luego cambar a que vengan de la BD)
        int nivelesActual = 6;
        int nivelesTotal = 10;

        int preguntas = 42;

        int temasActual = 3;
        int temasTotal = 5;

        int estrellasActual = 12;
        int estrellasTotal = 150;

        //APLICAR TODO
        SetProgreso(root, "ValorProgresoUno", "BarraUno", nivelesActual, nivelesTotal);
        SetSoloTexto(root, "ValorProgresoDos", preguntas); // sin barra real
        SetProgreso(root, "ValorProgresoTres", "BarraTres", temasActual, temasTotal);
        SetProgreso(root, "ValorProgresoCuatro", "BarraCuatro", estrellasActual, estrellasTotal);
    }

    void SetProgreso(VisualElement root, string nombreLabel, string nombreBarra, int actual, int total)
    {
        Label texto = root.Q<Label>(nombreLabel);
        VisualElement barra = root.Q<VisualElement>(nombreBarra);

        //sino existe lanza error
        if (texto == null || barra == null)
        {
            Debug.LogError("No se encontró: " + nombreLabel + " o " + nombreBarra);
            return;
        }

        //texto
        if (actual == total)
            texto.text = actual.ToString();
        else
            texto.text = actual + "/" + total;

        //porcentaje
        float porcentaje = (total == 0) ? 0 : (float)actual / total * 100f;

        //barra
        barra.style.width = Length.Percent(porcentaje);
    }

    void SetSoloTexto(VisualElement root, string nombreLabel, int valor)
    {
        Label texto = root.Q<Label>(nombreLabel);

        //sino existe lanza error
        if (texto == null)
        {
            Debug.LogError("No se encontró: " + nombreLabel);
            return;
        }

        //texto
        texto.text = valor.ToString();
    }
}