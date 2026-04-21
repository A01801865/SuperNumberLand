using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel3 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRTres;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonRTres = root.Q<Button>("RTres");

        if (botonRTres != null)
        {
            botonRTres.clicked += AbrirRNivel3;
        }
    }

    void OnDisable()
    {
        if (botonRTres != null)
        {
            botonRTres.clicked -= AbrirRNivel3;
        }
    }

    void AbrirRNivel3()
    {
        SceneManager.LoadScene("RNivel3");
    }
}