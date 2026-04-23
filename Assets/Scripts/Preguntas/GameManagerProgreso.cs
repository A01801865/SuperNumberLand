using UnityEngine;

public class GameManagerProgreso : MonoBehaviour
{
    public static GameManagerProgreso Instance;

    public int nivelActual = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AvanzarNivel()
    {
        nivelActual++;
    }

    public int ObtenerDificultad()
    {
        if (nivelActual <= 3) return 1;
        if (nivelActual <= 7) return 2;
        return 3;
    }
}