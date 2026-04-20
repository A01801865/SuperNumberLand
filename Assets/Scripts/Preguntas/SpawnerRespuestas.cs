using UnityEngine;

public class SpawnerRespuestas : MonoBehaviour
{
    public GameObject prefabCaja;
    public Transform[] puntosSpawn;
    public Puerta puerta;

    void Start()
    {
        int indiceCorrecto = Random.Range(0, puntosSpawn.Length);

        for (int i = 0; i < puntosSpawn.Length; i++)
        {
            GameObject obj = Instantiate(prefabCaja, puntosSpawn[i].position, Quaternion.identity);

            ObjetoRespuesta respuesta = obj.GetComponent<ObjetoRespuesta>();

            int numero = Random.Range(1, 10);

            respuesta.SetValor(numero.ToString(), i == indiceCorrecto, puerta);
        }
    }
}