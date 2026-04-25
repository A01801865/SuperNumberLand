using UnityEngine;
using System.Collections.Generic;

public class GameManagerProgreso : MonoBehaviour
{
    public static GameManagerProgreso Instance;

    public int nivelActual = 1;

    private List<string> nivelesDisponibles = new List<string>
    {
        "Mapa2", "Mapa3", "Mapa4", "Mapa5", "Mapa6",
        "Mapa7", "Mapa8", "Mapa9", "Mapa10", "Nivel1"
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string ObtenerSiguienteNivel(string nivelActualNombre)
    {
        // Si ya se jugaron todos, reiniciar la lista
        if (nivelesDisponibles.Count == 0)
        {
            nivelesDisponibles = new List<string>
            {
                "Mapa2", "Mapa3", "Mapa4", "Mapa5", "Mapa6",
                "Mapa7", "Mapa8", "Mapa9", "Mapa10", "Nivel1"
            };
        }

        // Quitar el nivel actual de los disponibles si está
        nivelesDisponibles.Remove(nivelActualNombre);

        // Elegir uno random de los que quedan
        int index = Random.Range(0, nivelesDisponibles.Count);
        string seleccionado = nivelesDisponibles[index];

        // Quitarlo para que no se repita
        nivelesDisponibles.RemoveAt(index);

        return seleccionado;
    }

    public void AvanzarNivel()
    {
        nivelActual++;
    }

    public int ObtenerDificultad()
    {
        if (nivelActual <= 3) return 1;
        if (nivelActual <= 7) return 2;
        return 3;
    }
}