using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel8 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDOcho;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonDOcho = root.Q<Button>("DOcho");

        if (botonDOcho != null)
        {
            botonDOcho.clicked += AbrirDNivel8;
        }
    }

    void OnDisable()
    {
        if (botonDOcho != null)
        {
            botonDOcho.clicked -= AbrirDNivel8;
        }
    }

    void AbrirDNivel8()
    {
        SceneManager.LoadScene("DNivel8");
    }
}