using UnityEngine;

public class SpawnerRespuestas : MonoBehaviour
{
    public GameObject prefabRespuesta;
    public Transform[] spawns;

    private GeneradorPreguntas generador;

    void Awake() 
    {
        generador = FindFirstObjectByType<GeneradorPreguntas>();
    }

    public void GenerarRespuestas()
    {
        if (generador == null)
            generador = FindFirstObjectByType<GeneradorPreguntas>(); 

        if (generador == null) return;

        int respuestaCorrecta = generador.RespuestaCorrecta;

        foreach (Transform hijo in transform)
        {
            Destroy(hijo.gameObject);
        }

        int indiceCorrecto = Random.Range(0, spawns.Length);

        for (int i = 0; i < spawns.Length; i++)
        {
            GameObject nueva = Instantiate(prefabRespuesta, spawns[i].position, Quaternion.identity);
            nueva.transform.SetParent(transform);

            ObjetoRespuesta obj = nueva.GetComponent<ObjetoRespuesta>();

            if (i == indiceCorrecto)
            {
                obj.SetValor(respuestaCorrecta);
            }
            else
            {
                int falsa;
                do
                {
                    falsa = respuestaCorrecta + Random.Range(-5, 6);
                    falsa = Mathf.Clamp(falsa, 0, 50);
                }
                while (falsa == respuestaCorrecta);

                obj.SetValor(falsa);
            }
        }
    }
}