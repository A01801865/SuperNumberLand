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
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);

        RespuestaCorrecta = a + b;

        Debug.Log("Respuesta correcta: " + RespuestaCorrecta);

        if (spawner != null)
            spawner.GenerarRespuestas();
        else
            Debug.LogError("Spawner es null en GenerarPregunta!");

        return a + " + " + b + " = ?";
    }
}