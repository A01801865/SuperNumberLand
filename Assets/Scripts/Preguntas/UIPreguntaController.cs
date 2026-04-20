using UnityEngine;
using UnityEngine.UIElements;

public class UIPreguntaController : MonoBehaviour
{
    public UIDocument uiDocument;

    Label preguntaLabel;

    void Start()
    {
        var root = uiDocument.rootVisualElement;

        preguntaLabel = root.Q<Label>("preguntaLabel");

        ActualizarPregunta();
    }

    public void ActualizarPregunta()
    {
        if (!GeneradorPreguntas.Instance.preguntaActiva) return;

        string pregunta = GeneradorPreguntas.Instance.GenerarPregunta();
        preguntaLabel.text = pregunta;
    }

    public void MostrarMensaje(string mensaje)
    {
        preguntaLabel.text = mensaje;
    }
}