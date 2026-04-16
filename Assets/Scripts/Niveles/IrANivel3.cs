using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel3 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonTres;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonTres = root.Q<Button>("Tres");

        if (botonTres != null)
        {
            botonTres.clicked += AbrirNivel3;
        }
    }

    void OnDisable()
    {
        if (botonTres != null)
        {
            botonTres.clicked -= AbrirNivel3;
        }
    }

    void AbrirNivel3()
    {
        SceneManager.LoadScene("Mapa3");
    }
}