using UnityEngine;
using UnityEngine.SceneManagement;

public class Puerta : MonoBehaviour
{
    private bool abierta = false;

    [Header("Escena siguiente")]
    public string siguienteNivel = "Mapa2";

    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError("La puerta necesita un Collider2D");
        }
    }

    public void Abrir()
    {
        if (abierta) return;

        abierta = true;

        Debug.Log("PUERTA ABIERTA");

        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!abierta) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Cambiando a: " + siguienteNivel);
            SceneManager.LoadScene(siguienteNivel);
        }
    }
}