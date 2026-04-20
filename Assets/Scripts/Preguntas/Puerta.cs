using UnityEngine;
using UnityEngine.SceneManagement;

public class Puerta : MonoBehaviour
{
    private bool abierta = false;

    public string siguienteNivel = "Mapa2"; 

    public void Abrir()
    {
        if (abierta) return;

        abierta = true;

        Debug.Log("PUERTA   ABIERTA");

        
        Collider2D col = GetComponent<Collider2D>();
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