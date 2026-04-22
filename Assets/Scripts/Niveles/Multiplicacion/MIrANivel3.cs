using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel3 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMTres;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonMTres = root.Q<Button>("MTres");

        if (botonMTres != null)
        {
            botonMTres.clicked += AbrirMNivel3;
        }
    }

    void OnDisable()
    {
        if (botonMTres != null)
        {
            botonMTres.clicked -= AbrirMNivel3;
        }
    }

    void AbrirMNivel3()
    {
        SceneManager.LoadScene("MNivel3");
    }
}