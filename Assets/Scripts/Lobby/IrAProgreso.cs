using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrAProgreso : MonoBehaviour
{
    private UIDocument menu;
    private Button boton;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        boton = root.Q<Button>("Progreso");
        if (boton != null)
        {
            boton.clicked += Abrir;
        }
    }

    void OnDisable()
    {
        if (boton != null)
        {
            boton.clicked -= Abrir;
        }
    }

    void Abrir()
    {
        SceneManager.LoadScene("Progreso");
    }
}