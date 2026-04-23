using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel10 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDDiez;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonDDiez = root.Q<Button>("DDiez");

        if (botonDDiez != null)
        {
            botonDDiez.clicked += AbrirDNivel10;
        }
    }

    void OnDisable()
    {
        if (botonDDiez != null)
        {
            botonDDiez.clicked -= AbrirDNivel10;
        }
    }

    void AbrirDNivel10()
    {
        SceneManager.LoadScene("DNivel10");
    }
}