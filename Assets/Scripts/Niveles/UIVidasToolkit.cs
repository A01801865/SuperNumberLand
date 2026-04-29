using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIVidasToolkit : MonoBehaviour
{
    private VisualElement vida1;
    private VisualElement vida2;
    private VisualElement vida3;
    private VisualElement pantallaPerder;
    private VisualElement pantallaGanar;
    private VisualElement estrella1;
    private VisualElement estrella2;
    private VisualElement estrella3;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        vida1 = root.Q<VisualElement>("Vida_1");
        vida2 = root.Q<VisualElement>("Vida_2");
        vida3 = root.Q<VisualElement>("Vida_3");

        pantallaPerder = root.Q<VisualElement>("Perder");
        pantallaGanar = root.Q<VisualElement>("Ganar");

        estrella1 = root.Q<VisualElement>("Estrella_1");
        estrella2 = root.Q<VisualElement>("Estrella_2");
        estrella3 = root.Q<VisualElement>("Estrella_3");

        var fondoPerder = root.Q<VisualElement>("FondoPerder");
        if (fondoPerder != null)
        {
            Button botonReintentar = fondoPerder.Q<Button>("BotonReintentar");
            if (botonReintentar != null)
                botonReintentar.clicked += Reintentar;

            Button botonVolverPerder = fondoPerder.Q<Button>("BotonVolve");
            if (botonVolverPerder != null)
                botonVolverPerder.clicked += VolverMenu;
        }

        if (pantallaPerder != null)
            pantallaPerder.style.display = DisplayStyle.None;

        if (pantallaGanar != null)
            pantallaGanar.style.display = DisplayStyle.None;

        if (GameManagerProgreso.Instance != null)
            ActualizarVidas(GameManagerProgreso.Instance.vidasActuales);
    }

    private void Reintentar()
    {
        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = 3;
            GameManagerProgreso.Instance.nivelActual = 1;
            GameManagerProgreso.Instance.vidasPerdidas = 0;
        }

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        Time.timeScale = 1f;

        // ← Reintentar desde el primer nivel del tipo actual
        if (GameManagerProgreso.Instance != null)
        {
            switch (GameManagerProgreso.Instance.tipoActual)
            {
                case GameManagerProgreso.TipoNivel.Suma:
                    SceneManager.LoadScene("Nivel1");
                    break;
                case GameManagerProgreso.TipoNivel.Resta:
                    SceneManager.LoadScene("RNivel1");
                    break;
                case GameManagerProgreso.TipoNivel.Multiplicacion:
                    SceneManager.LoadScene("MNivel1");
                    break;
                case GameManagerProgreso.TipoNivel.Division:
                    SceneManager.LoadScene("DNivel1");
                    break;
                default:
                    SceneManager.LoadScene("Nivel1");
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene("Nivel1");
        }
    }

    private void VolverMenu()
    {
        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = 3;
            GameManagerProgreso.Instance.nivelActual = 1;
            GameManagerProgreso.Instance.vidasPerdidas = 0;
        }

        if (MonedaManager.instance != null)
            MonedaManager.instance.totalMonedas = 0;

        Time.timeScale = 1f;

        // ← Regresar al menú correcto según el tipo de nivel
        if (GameManagerProgreso.Instance != null)
        {
            switch (GameManagerProgreso.Instance.tipoActual)
            {
                case GameManagerProgreso.TipoNivel.Suma:
                    SceneManager.LoadScene("Niveles");
                    break;
                case GameManagerProgreso.TipoNivel.Resta:
                    SceneManager.LoadScene("NivelesResta");
                    break;
                case GameManagerProgreso.TipoNivel.Multiplicacion:
                    SceneManager.LoadScene("NivelesMulti");
                    break;
                case GameManagerProgreso.TipoNivel.Division:
                    SceneManager.LoadScene("NivelesDivi");
                    break;
                default:
                    SceneManager.LoadScene("Niveles");
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene("Niveles");
        }
    }

    public void ActualizarVidas(int vidas)
    {
        if (vida1 != null)
            vida1.style.display = vidas >= 1 ? DisplayStyle.Flex : DisplayStyle.None;
        if (vida2 != null)
            vida2.style.display = vidas >= 2 ? DisplayStyle.Flex : DisplayStyle.None;
        if (vida3 != null)
            vida3.style.display = vidas >= 3 ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void MostrarPantallaPerder()
    {
        Debug.Log("MOSTRANDO PANTALLA DE PERDER");
        if (pantallaPerder != null)
            pantallaPerder.style.display = DisplayStyle.Flex;
        else
            Debug.LogError("No se encontró Perder");
    }

    public void MostrarPantallaGanar()
    {
        Debug.Log("MOSTRANDO PANTALLA DE GANAR");

        if (pantallaGanar != null)
            pantallaGanar.style.display = DisplayStyle.Flex;
        else
            Debug.LogError("No se encontró Ganar");

        int estrellas = GameManagerProgreso.Instance != null
            ? GameManagerProgreso.Instance.CalcularEstrellas()
            : 1;

        if (estrella1 != null)
            estrella1.style.display = estrellas >= 1 ? DisplayStyle.Flex : DisplayStyle.None;
        if (estrella3 != null)
            estrella3.style.display = estrellas >= 2 ? DisplayStyle.Flex : DisplayStyle.None;
        if (estrella2 != null)
            estrella2.style.display = estrellas >= 3 ? DisplayStyle.Flex : DisplayStyle.None;
    }
}