using UnityEngine;

public class SpawnerRespuestas : MonoBehaviour
{
    public GameObject prefabCaja;
    public Transform[] puntosSpawn;
    public Puerta puerta;

    void Start()
    {
        int correcta = GeneradorPreguntas.Instance.respuestaCorrecta;

        int indiceCorrecto = Random.Range(0, puntosSpawn.Length);

        for (int i = 0; i < puntosSpawn.Length; i++)
        {
            GameObject obj = Instantiate(prefabCaja, puntosSpawn[i].position, Quaternion.identity);

            ObjetoRespuesta respuesta = obj.GetComponent<ObjetoRespuesta>();

            int valor;

            if (i == indiceCorrecto)
            {
                valor = correcta;
            }
            else
            {
                valor = correcta + Random.Range(-3, 4);

                if (valor == correcta)
                    valor += 1;
            }

            respuesta.SetValor(valor.ToString(), i == indiceCorrecto, puerta);
        }
    }
}