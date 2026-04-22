using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel5 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMCinco;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonMCinco = root.Q<Button>("MCinco");

        if (botonMCinco != null)
        {
            botonMCinco.clicked += AbrirMNivel5;
        }
    }

    void OnDisable()
    {
        if (botonMCinco != null)
        {
            botonMCinco.clicked -= AbrirMNivel5;
        }
    }

    void AbrirMNivel5()
    {
        SceneManager.LoadScene("MNivel5");
    }
}