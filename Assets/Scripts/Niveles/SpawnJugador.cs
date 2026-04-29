using UnityEngine;

public class SpawnJugador : MonoBehaviour
{
    public GameObject[] personajes;
    public Transform puntoSpawn;

    void Start()
    {
        int index = 0;

        if (GameManager.instancia != null)
        {
            index = GameManager.instancia.personajeSeleccionado;
        }
        else
        {
            // Si no hay GameManager, leer desde PlayerPrefs
            int id_item = PlayerPrefs.GetInt("personaje_seleccionado", 1);
            index = ObtenerIndex(id_item);
            Debug.LogWarning("GameManager no encontrado, leyendo de PlayerPrefs: " + index);
        }

        if (personajes.Length > index)
            Instantiate(personajes[index], puntoSpawn.position, Quaternion.identity);
        else
            Debug.LogError("No hay personaje en el índice: " + index);
    }

    int ObtenerIndex(int id_item)
    {
        switch (id_item)
        {
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            default: return 0;
        }
    }
}