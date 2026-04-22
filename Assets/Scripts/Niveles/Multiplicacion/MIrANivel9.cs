using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel9 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMNueve;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonMNueve = root.Q<Button>("MNueve");

        if (botonMNueve != null)
        {
            botonMNueve.clicked += AbrirMNivel9;
        }
    }

    void OnDisable()
    {
        if (botonMNueve != null)
        {
            botonMNueve.clicked -= AbrirMNivel9;
        }
    }

    void AbrirMNivel9()
    {
        SceneManager.LoadScene("MNivel9");
    }
}