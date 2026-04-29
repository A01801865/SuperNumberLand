using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIResultadoController : MonoBehaviour
{
    private Button btnReintentar;
    private Button btnVolver;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        btnReintentar = root.Q<Button>("BotonReintentar");
        btnVolver = root.Q<Button>("BotonVolve"); // así se llama en el UXML

        if (btnReintentar != null)
            btnReintentar.clicked += Reintentar;

        if (btnVolver != null)
            btnVolver.clicked += VolverMenu;
    }

    void Reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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