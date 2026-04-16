using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel2 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDos;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonDos = root.Q<Button>("Dos");

        if (botonDos != null)
        {
            botonDos.clicked += AbrirNivel2;
        }
    }

    void OnDisable()
    {
        if (botonDos != null)
        {
            botonDos.clicked -= AbrirNivel2;
        }
    }

    void AbrirNivel2()
    {
        SceneManager.LoadScene("Mapa2");
    }
}