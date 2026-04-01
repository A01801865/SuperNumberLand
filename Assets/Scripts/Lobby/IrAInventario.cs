using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrAInventario : MonoBehaviour
{
    private UIDocument menu;
    private Button boton;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        boton = root.Q<Button>("Inventario");
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
        SceneManager.LoadScene("Inventario");
    }
}