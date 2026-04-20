using UnityEngine;

public class GeneradorPreguntas : MonoBehaviour
{
    public static GeneradorPreguntas Instance;

    public int respuestaCorrecta;
    public bool preguntaActiva = true; 

    void Awake()
    {
        Instance = this;
    }

    public string GenerarPregunta()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);

        respuestaCorrecta = a + b;

        return a + " + " + b + " = ?";
    }
}