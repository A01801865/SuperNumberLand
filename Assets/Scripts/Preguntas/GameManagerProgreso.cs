using UnityEngine;
using System.Collections.Generic;

public class GameManagerProgreso : MonoBehaviour
{
    public static GameManagerProgreso Instance;

    public int nivelActual = 1;
    public int totalNiveles = 10;
    public int vidasActuales = 3;
    public int vidasPerdidas = 0;
    public int mapasCompletados = 0;
    public int mapasPorNivel = 5;

    public enum TipoNivel { Suma, Resta, Multiplicacion, Division }
    public TipoNivel tipoActual;

    private List<string> nivelesSuma = new List<string>
    {
        "Nivel1", "Mapa2","Mapa3","Mapa4","Mapa5",
        "Mapa6","Mapa7","Mapa8","Mapa9","Mapa10"
    };

    private List<string> nivelesResta = new List<string>
    {
        "RNivel1","RNivel2","RNivel3","RNivel4","RNivel5",
        "RNivel6","RNivel7","RNivel8","RNivel9","RNivel10"
    };

    private List<string> nivelesMultiplicacion = new List<string>
    {
        "MNivel1","MNivel2","MNivel3","MNivel4","MNivel5",
        "MNivel6","MNivel7","MNivel8","MNivel9","MNivel10"
    };

    private List<string> nivelesDivision = new List<string>
    {
        "DNivel1","DNivel2","DNivel3","DNivel4","DNivel5",
        "DNivel6","DNivel7","DNivel8","DNivel9","DNivel10"
    };

    private List<string> nivelesRestantes = new List<string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InicializarLista();
    }

    void InicializarLista()
    {
        nivelesRestantes.Clear();

        switch (tipoActual)
        {
            case TipoNivel.Suma: nivelesRestantes.AddRange(nivelesSuma); break;
            case TipoNivel.Resta: nivelesRestantes.AddRange(nivelesResta); break;
            case TipoNivel.Multiplicacion: nivelesRestantes.AddRange(nivelesMultiplicacion); break;
            case TipoNivel.Division: nivelesRestantes.AddRange(nivelesDivision); break;
        }
    }

    public string ObtenerSiguienteNivel(string nivelActualNombre)
    {
        if (nivelesRestantes.Count == 0)
            InicializarLista();

        nivelesRestantes.Remove(nivelActualNombre);

        if (nivelesRestantes.Count == 0)
            InicializarLista();

        int index = Random.Range(0, nivelesRestantes.Count);
        string siguiente = nivelesRestantes[index];
        nivelesRestantes.RemoveAt(index);

        return siguiente;
    }

    public void AvanzarNivel()
    {
        mapasCompletados++;
    }

    public bool HaGanado()
    {
        return mapasCompletados >= mapasPorNivel;
    }

    public void ResetearMapas()
    {
        mapasCompletados = 0;
        vidasActuales = 3;
        vidasPerdidas = 0;
    }

    public int ObtenerDificultad()
    {
        if (nivelActual <= 3) return 1;
        if (nivelActual <= 7) return 2;
        return 3;
    }

    public void SetTipoNivel(TipoNivel tipo)
    {
        tipoActual = tipo;
        InicializarLista();
    }

    public int CalcularEstrellas()
    {
        return vidasActuales;
    }
}