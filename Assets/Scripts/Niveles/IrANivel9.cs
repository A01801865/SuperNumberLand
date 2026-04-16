using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel9 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonNueve;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonNueve = root.Q<Button>("Nueve");

        if (botonNueve != null)
        {
            botonNueve.clicked += AbrirNivel9;
        }
    }

    void OnDisable()
    {
        if (botonNueve != null)
        {
            botonNueve.clicked -= AbrirNivel9;
        }
    }

    void AbrirNivel9()
    {
        SceneManager.LoadScene("Mapa9");
    }
}

