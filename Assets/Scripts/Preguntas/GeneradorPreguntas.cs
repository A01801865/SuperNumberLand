using UnityEngine;

public class GeneradorPreguntas : MonoBehaviour
{
    public enum TipoOperacion
    {
        Suma,
        Resta,
        Multiplicacion,
        Division
    }

    public TipoOperacion tipoOperacion;

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

        switch (tipoOperacion)
        {
            case TipoOperacion.Suma:
                RespuestaCorrecta = a + b;
                break;

            case TipoOperacion.Resta:
                if (a < b)
                {
                    int temp = a;
                    a = b;
                    b = temp;
                }
                RespuestaCorrecta = a - b;
                break;

            case TipoOperacion.Multiplicacion:
                RespuestaCorrecta = a * b;
                break;

            case TipoOperacion.Division:
                RespuestaCorrecta = Random.Range(1, 10);
                b = Random.Range(1, 10);
                a = RespuestaCorrecta * b;
                break;
        }

        Debug.Log("Respuesta correcta: " + RespuestaCorrecta);

        if (spawner != null)
            spawner.GenerarRespuestas();
        else
            Debug.LogError("Spawner es null en GenerarPregunta!");

        // Generar texto dinámico
        string simbolo = "+";

        switch (tipoOperacion)
        {
            case TipoOperacion.Suma: simbolo = "+"; break;
            case TipoOperacion.Resta: simbolo = "-"; break;
            case TipoOperacion.Multiplicacion: simbolo = "×"; break;
            case TipoOperacion.Division: simbolo = "÷"; break;
        }

        return a + " " + simbolo + " " + b + " = ?";
    }
}