using UnityEngine;
using TMPro;

public class GameManagerPreguntas : MonoBehaviour
{
    public TextMeshProUGUI textoPregunta;

    public enum TipoOperacion
    {
        Suma,
        Resta,
        Multiplicacion,
        Division
    }

    public TipoOperacion tipoOperacion;

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

        switch (tipoOperacion)
        {
            case TipoOperacion.Suma:
                respuestaCorrecta = a + b;
                textoPregunta.text = a + " mas " + b + " = ?";
                break;

            case TipoOperacion.Resta:
                respuestaCorrecta = a - b;
                textoPregunta.text = a + " menos " + b + " = ?";
                break;

            case TipoOperacion.Multiplicacion:
                respuestaCorrecta = a * b;
                textoPregunta.text = a + " X " + b + " = ?";
                break;

            case TipoOperacion.Division:
                // Evitar decimales
                respuestaCorrecta = Random.Range(1, 10);
                b = Random.Range(1, 10);
                a = respuestaCorrecta * b;

                textoPregunta.text = a + " / " + b + " = ?";
                break;
        }
    }
}