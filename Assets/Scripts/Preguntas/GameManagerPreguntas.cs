using UnityEngine;

public class GameManagerPreguntas : MonoBehaviour
{
    public static GameManagerPreguntas instancia;

    public int preguntaActual = 0;
    public int totalPreguntas = 5;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SiguientePregunta()
    {
        preguntaActual++;
    }

    public bool NivelCompletado()
    {
        return preguntaActual >= totalPreguntas;
    }
}