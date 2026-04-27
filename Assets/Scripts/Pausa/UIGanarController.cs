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

        // Registrar callbacks directamente en los botones
        var botonSiguiente = fondoGanar?.Q<Button>("BotonSigNiv");
        var botonVolver = fondoGanar?.Q<Button>("BotonVuelve");

        if (botonSiguiente != null)
            botonSiguiente.RegisterCallback<ClickEvent>(e => SiguienteNivel());

        if (botonVolver != null)
            botonVolver.RegisterCallback<ClickEvent>(e => VolverMenu());
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