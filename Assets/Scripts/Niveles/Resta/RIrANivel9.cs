using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel9 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRNueve;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonRNueve = root.Q<Button>("RNueve");

        if (botonRNueve != null)
        {
            botonRNueve.clicked += AbrirRNivel9;
        }
    }

    void OnDisable()
    {
        if (botonRNueve != null)
        {
            botonRNueve.clicked -= AbrirRNivel9;
        }
    }

    void AbrirRNivel9()
    {
        SceneManager.LoadScene("RNivel9");
    }
}