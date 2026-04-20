using UnityEngine;

public class SpawnJugador : MonoBehaviour
{
    public GameObject[] personajes;
    public Transform puntoSpawn;

    void Start()
    {
        int index = GameManager.instancia.personajeSeleccionado;

        Instantiate(personajes[index], puntoSpawn.position, Quaternion.identity);
    }
}