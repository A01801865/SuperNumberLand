using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel4 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonCuatro;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonCuatro = root.Q<Button>("Cuatro");

        if (botonCuatro != null)
        {
            botonCuatro.clicked += AbrirNivel4;
        }
    }

    void OnDisable()
    {
        if (botonCuatro != null)
        {
            botonCuatro.clicked -= AbrirNivel4;
        }
    }

    void AbrirNivel4()
    {
        SceneManager.LoadScene("Mapa4");
    }
}