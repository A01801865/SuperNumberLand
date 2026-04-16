using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel8 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonOcho;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonOcho = root.Q<Button>("Ocho");

        if (botonOcho != null)
        {
            botonOcho.clicked += AbrirNivel8;
        }
    }

    void OnDisable()
    {
        if (botonOcho != null)
        {
            botonOcho.clicked -= AbrirNivel8;
        }
    }

    void AbrirNivel8()
    {
        SceneManager.LoadScene("Mapa8");
    }
}
