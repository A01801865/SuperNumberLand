using UnityEngine;

public class SpawnJugador : MonoBehaviour
{
    public GameObject[] personajes;
    public Transform puntoSpawn;

    void Start()
    {
        int index = 0; // ← Default si no hay GameManager

        if (GameManager.instancia != null)
            index = GameManager.instancia.personajeSeleccionado;
        else
            Debug.LogWarning("GameManager no encontrado, usando personaje 0");

        if (personajes.Length > index)
            Instantiate(personajes[index], puntoSpawn.position, Quaternion.identity);
        else
            Debug.LogError("No hay personaje en el índice: " + index);
    }
}