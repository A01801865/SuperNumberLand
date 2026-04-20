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

    public void RecibirGolpe()
    {
        if (esCorrecta)
        {
            Debug.Log("Correcto");

            
            GeneradorPreguntas.Instance.preguntaActiva = false;

            
            GameObject[] cajas = GameObject.FindGameObjectsWithTag("Respuesta");

            foreach (GameObject caja in cajas)
            {
                Destroy(caja);
            }

            
            puerta.Abrir();

            
            FindObjectOfType<UIPreguntaController>().MostrarMensaje("PUERTA   ABIERTA");
        }
        else
        {
            Debug.Log("Incorrecto");

            PlayerController player = FindObjectOfType<PlayerController>();

            if (player != null)
            {
                player.PerderVida();
            }
        }
    }
}