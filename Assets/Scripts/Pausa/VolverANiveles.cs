using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class VolverANiveles : MonoBehaviour
{
    private UIDocument menu;
    private Button botonVolver;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonVolver = root.Q<Button>("BotonVolver");

        if (botonVolver != null)
        {
            botonVolver.clicked += Volver;
        }
    }

    void OnDisable()
    {
        if (botonVolver != null)
        {
            botonVolver.clicked -= Volver;
        }
    }

    void Volver()
    {
        SceneManager.LoadScene("Niveles");
    }
}