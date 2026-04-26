using UnityEngine;

public class GeneradorPreguntas : MonoBehaviour
{
    public int RespuestaCorrecta;
    public SpawnerRespuestas spawner;

    void Awake()
    {
        if (spawner == null)
            spawner = FindFirstObjectByType<SpawnerRespuestas>();
    }

    public string GenerarPregunta()
    {
        int dificultad = GameManagerProgreso.Instance != null
            ? GameManagerProgreso.Instance.ObtenerDificultad()
            : 1;

        int a, b;

        // Dificultad 1 (niveles 1-3): números del 1 al 5
        // Dificultad 2 (niveles 4-7): números del 1 al 10
        // Dificultad 3 (niveles 8-10): números del 5 al 20
        if (dificultad == 1)
        {
            a = Random.Range(1, 6);
            b = Random.Range(1, 6);
        }
        else if (dificultad == 2)
        {
            a = Random.Range(1, 11);
            b = Random.Range(1, 11);
        }
        else
        {
            a = Random.Range(5, 21);
            b = Random.Range(5, 21);
        }

        RespuestaCorrecta = a + b;

        Debug.Log("Dificultad: " + dificultad + " | Respuesta correcta: " + RespuestaCorrecta);

        if (spawner != null)
            spawner.GenerarRespuestas();
        else
            Debug.LogError("Spawner es null!");

        return "Resuelve: " + a + " + " + b + " = ?";
    }
}