using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANiveles : MonoBehaviour
{
    private UIDocument menu;
    private Button botonSuma;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonSuma = root.Q<Button>("BotonSuma");

        if (botonSuma != null)
        {
            botonSuma.clicked += AbrirNiveles;
        }
    }

    void OnDisable()
    {
        if (botonSuma != null)
        {
            botonSuma.clicked -= AbrirNiveles;
        }
    }

    void AbrirNiveles()
    {
        SceneManager.LoadScene("Niveles");
    }
}