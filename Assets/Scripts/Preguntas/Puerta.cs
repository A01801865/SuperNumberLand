using UnityEngine;
using UnityEngine.SceneManagement;

public class Puerta : MonoBehaviour
{
    private bool abierta = false;
    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
        if (col == null)
            Debug.LogError("La puerta necesita un Collider2D");
    }

    public void Abrir()
    {
        if (abierta) return;
        abierta = true;

        if (col != null)
            col.isTrigger = true;

        Debug.Log("¡Puerta abierta!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!abierta) return;

        if (collision.CompareTag("Player"))
        {
            if (GameManagerProgreso.Instance != null)
            {
                GameManagerProgreso.Instance.AvanzarNivel();

                if (GameManagerProgreso.Instance.HaGanado())
                {
                    Debug.Log("¡GANASTE!");

                    // Detener al jugador
                    PlayerController player = FindFirstObjectByType<PlayerController>();
                    if (player != null)
                        player.Detener();

                    // Obtener vidas actuales como estrellas
                    int estrellas = Mathf.Clamp(GameManagerProgreso.Instance.vidasActuales, 0, 3);

                    // Mostrar pantalla de ganar con estrellas correctas
                    UIVidasToolkit ui = FindFirstObjectByType<UIVidasToolkit>();
                    if (ui != null)
                        ui.MostrarPantallaGanar(estrellas);
                    else
                        Debug.LogError("No se encontró UIVidasToolkit");

                    return;
                }

                // Cargar siguiente nivel
                string nivelActual = SceneManager.GetActiveScene().name;
                string siguiente = GameManagerProgreso.Instance.ObtenerSiguienteNivel(nivelActual);
                SceneManager.LoadScene(siguiente);
            }
        }
    }
}