using UnityEngine;
using TMPro;

public class GameManagerPreguntas : MonoBehaviour
{
    public TextMeshProUGUI textoPregunta;

    private int respuestaCorrecta;

    void Start()
    {
        GenerarPregunta();
    }

    public int GetRespuestaCorrecta()
    {
        return respuestaCorrecta;
    }

    void GenerarPregunta()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);

        respuestaCorrecta = a + b;

        textoPregunta.text = a + " + " + b + " = ?";
    }
}