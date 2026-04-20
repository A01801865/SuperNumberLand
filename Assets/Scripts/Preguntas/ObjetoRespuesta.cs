using UnityEngine;
using TMPro;

public class ObjetoRespuesta : MonoBehaviour
{
    public bool esCorrecta;
    public Puerta puerta;
    public TextMeshPro texto;

    public void SetValor(string valor, bool correcta, Puerta p)
    {
        texto.text = valor;
        esCorrecta = correcta;
        puerta = p;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (esCorrecta)
            {
                Debug.Log("Correcto");
                puerta.Abrir();
            }
            else
            {
                Debug.Log("Incorrecto");
            }

            Destroy(gameObject); // opcional: desaparece al tocarla
        }
    }
}