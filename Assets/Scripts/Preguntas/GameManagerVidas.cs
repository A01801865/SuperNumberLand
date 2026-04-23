using UnityEngine;

public class GameManagerVidas : MonoBehaviour
{
    public static GameManagerVidas Instance;

    public int vidas = 3;

    public UIVidasToolkit ui; 

    void Awake()
    {
        Instance = this;
    }

    public void PerderVida()
    {
        vidas--;

        Debug.Log("Vida perdida. Vidas actuales: " + vidas);

        
        if (ui != null)
        {
            ui.ActualizarVidas(vidas);
        }

        if (vidas <= 0)
        {
            Debug.Log("Game Over");

            if (ui != null)
            {
                ui.MostrarPantallaPerder();
            }
        }
    }
}