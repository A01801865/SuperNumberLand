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
            string nivelActual = SceneManager.GetActiveScene().name;

            if (GameManagerProgreso.Instance != null)
            {
                GameManagerProgreso.Instance.AvanzarNivel();
                string siguiente = GameManagerProgreso.Instance.ObtenerSiguienteNivel(nivelActual);
                SceneManager.LoadScene(siguiente);
            }
        }
    }
}