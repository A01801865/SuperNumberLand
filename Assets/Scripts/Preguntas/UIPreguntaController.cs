using UnityEngine;
using UnityEngine.UIElements;

public class UIPreguntaController : MonoBehaviour
{
    private Label preguntaLabel;
    private GeneradorPreguntas generador;
    private string preguntaActual;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        preguntaLabel = root.Q<Label>("preguntaLabel");

        generador = FindFirstObjectByType<GeneradorPreguntas>();

        ActualizarPregunta();
    }

    public void ActualizarPregunta()
    {
        if (generador == null) return;

        preguntaActual = generador.GenerarPregunta();

        if (preguntaLabel != null)
            preguntaLabel.text = preguntaActual;
    }

    public void MostrarIncorrecto()
    {
        if (preguntaLabel != null)
            preguntaLabel.text = "¡Fallaste!";

        Invoke("RestaurarPregunta", 2f);
    }

    private void RestaurarPregunta()
    {
        if (preguntaLabel != null)
            preguntaLabel.text = preguntaActual;
    }

    public void MostrarPuertaAbierta()
    {
        if (preguntaLabel != null)
            preguntaLabel.text = "¡Puerta  abierta!";
    }
}