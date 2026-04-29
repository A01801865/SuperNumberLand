using UnityEngine;
using UnityEngine.UIElements;

public class UINivelActual : MonoBehaviour
{
    private Label nivelLabel;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        nivelLabel = root.Q<Label>("NivelActual");

        ActualizarNivel();
    }

    public void ActualizarNivel()
    {
        if (nivelLabel == null) return;

        int pregunta = GameManagerProgreso.Instance != null
            ? GameManagerProgreso.Instance.mapasCompletados + 1
            : 1;

        nivelLabel.text = "Pregunta " + pregunta;
    }
}