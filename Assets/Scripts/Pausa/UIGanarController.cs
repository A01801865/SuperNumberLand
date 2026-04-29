using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIGanarController : MonoBehaviour
{
    private VisualElement fondoGanar;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        fondoGanar = root.Q<VisualElement>("FondoGanar");

        var botonSiguiente = fondoGanar?.Q<Button>("BotonSigNiv");
        var botonVolver    = fondoGanar?.Q<Button>("BotonVuelve");

        if (botonSiguiente != null)
            botonSiguiente.RegisterCallback<ClickEvent>(e => SiguienteNivel());

        if (botonVolver != null)
            botonVolver.RegisterCallback<ClickEvent>(e => VolverMenu());
    }

    private void SiguienteNivel()
    {
        Debug.Log("SiguienteNivel clicked!");

        int nivelActual = PlayerPrefs.GetInt("nivel_seleccionado", 1);
        int siguienteNivel = nivelActual + 1;

        GameManagerProgreso.Instance?.ResetearMapas();

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        Time.timeScale = 1f;

        if (siguienteNivel <= 10)
        {
            PlayerPrefs.SetInt("nivel_seleccionado", siguienteNivel);
            PlayerPrefs.Save();
            string tipo = PlayerPrefs.GetString("tipo_nivel", "suma");
            string escena = ObtenerPrimerMapa(siguienteNivel, tipo);
            SceneManager.LoadScene(escena);
        }
        else
        {
            SceneManager.LoadScene(ObtenerEscenaNiveles());
        }
    }

    private void VolverMenu()
    {
        Debug.Log("VolverMenu clicked!");

        GameManagerProgreso.Instance?.ResetearMapas();

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        Time.timeScale = 1f;
        SceneManager.LoadScene(ObtenerEscenaNiveles());
    }

    string ObtenerEscenaNiveles()
    {
        string tipo = PlayerPrefs.GetString("tipo_nivel", "suma");
        switch (tipo)
        {
            case "resta":         return "NivelesResta";
            case "multiplicacion": return "NivelesMulti";
            case "division":      return "NivelesDivi";
            default:              return "Niveles";
        }
    }

    string ObtenerPrimerMapa(int nivel, string tipo)
    {
        switch (tipo)
        {
            case "suma":
                return nivel == 1 ? "Nivel1" : $"Mapa{nivel}";
            case "resta":
                return $"RNivel{nivel}";
            case "multiplicacion":
                return $"MNivel{nivel}";
            case "division":
                return $"DNivel{nivel}";
            default:
                return "Niveles";
        }
    }
}