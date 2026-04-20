using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public int personajeSeleccionado = 0; // default

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
}