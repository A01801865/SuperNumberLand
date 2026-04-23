using UnityEngine;
using UnityEngine.SceneManagement;

public class Puerta : MonoBehaviour
{
    private bool abierta = false;
    private Collider2D col;

    
    private string[] nivelesSuma = new string[]
    {
        "Mapa2",
        "Mapa3",
        "Mapa4",
        "Mapa5",
        "Mapa6",
        "Mapa7",
        "Mapa8",
        "Mapa9",
        "Mapa10"
    };

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
            string nivelActual = SceneManager.GetActiveScene().name;

            string nivelSeleccionado;

            do
            {
                int index = Random.Range(0, nivelesSuma.Length);
                nivelSeleccionado = nivelesSuma[index];
            }
            while (nivelSeleccionado == nivelActual);

            if (GameManagerProgreso.Instance != null)
                GameManagerProgreso.Instance.AvanzarNivel();

            SceneManager.LoadScene(nivelSeleccionado);
        }
    }
}