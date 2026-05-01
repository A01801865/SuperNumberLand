using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIResultadoController : MonoBehaviour
{
    private Button btnReintentar;
    private Button btnVolver;

    void OnEnable()
    {
        // Obtener los botones desde la UI y suscribirlos a sus métodos
        var root = GetComponent<UIDocument>().rootVisualElement;

        btnReintentar = root.Q<Button>("BotonReintentar");
        btnVolver     = root.Q<Button>("BotonVolve"); // nombre definido en el UXML

        if (btnReintentar != null)
            btnReintentar.clicked += Reintentar;

        if (btnVolver != null)
            btnVolver.clicked += VolverMenu;
    }

    // Recarga la escena actual para volver a intentar el nivel
    void Reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Regresa al menú de niveles correspondiente según el tipo de operación que se estaba jugando
    void VolverMenu()
    {
        string tipo = PlayerPrefs.GetString("tipo_nivel", "suma").ToLower();
        switch (tipo)
        {
            case "resta":          SceneManager.LoadScene("NivelesResta"); break;
            case "multiplicacion": SceneManager.LoadScene("NivelesMulti"); break;
            case "division":       SceneManager.LoadScene("NivelesDivi");  break;
            default:               SceneManager.LoadScene("Niveles");      break;
        }
    }
}