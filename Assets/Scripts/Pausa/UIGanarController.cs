using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIGanarController : MonoBehaviour
{
    private Button botonSiguiente;
    private Button botonVolver;

    void Start() // ← OnEnable → Start
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var fondoGanar = root.Q<VisualElement>("FondoGanar");
        Debug.Log("FondoGanar: " + fondoGanar);

        if (fondoGanar != null)
        {
            botonSiguiente = fondoGanar.Q<Button>("BotonSigNiv");
            botonVolver = fondoGanar.Q<Button>("BotonVolver");

            Debug.Log("BotonSigNiv: " + botonSiguiente);
            Debug.Log("BotonVolver: " + botonVolver);

            if (botonSiguiente != null)
                botonSiguiente.clicked += SiguienteNivel;

            if (botonVolver != null)
                botonVolver.clicked += VolverMenu;
        }
        else
            Debug.LogError("No se encontró FondoGanar");
    }

    private void SiguienteNivel()
    {
        Debug.Log("SiguienteNivel clicked!");

        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = 3;
            GameManagerProgreso.Instance.nivelActual = 1;
            GameManagerProgreso.Instance.vidasPerdidas = 0;
        }

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Mapa2");
    }

    private void VolverMenu()
    {
        Debug.Log("VolverMenu clicked!");

        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = 3;
            GameManagerProgreso.Instance.nivelActual = 1;
            GameManagerProgreso.Instance.vidasPerdidas = 0;
        }

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Niveles");
    }
}