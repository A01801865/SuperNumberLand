using UnityEngine;

public class GameManagerVidas : MonoBehaviour
{
    public static GameManagerVidas Instance;

    public int vidas = 3;

    void Awake()
    {
        Instance = this;
    }

    public void PerderVida()
    {
        vidas--;
        Debug.Log("Vidas: " + vidas);

        if (vidas <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}