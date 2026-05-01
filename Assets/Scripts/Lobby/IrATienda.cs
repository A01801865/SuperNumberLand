using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrATienda : MonoBehaviour
{
    private UIDocument menu;
    private Button boton;

    void OnEnable()
    {
        // Obtener el botón y suscribirlo al método para abrir la tienda
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        boton = root.Q<Button>("Tienda");
        if (boton != null)
            boton.clicked += Abrir;
    }

    void OnDisable()
    {
        // Desuscribir el botón al deshabilitar el objeto para evitar llamadas duplicadas
        if (boton != null)
            boton.clicked -= Abrir;
    }

    // Carga la escena de la tienda
    void Abrir()
    {
        SceneManager.LoadScene("Tienda");
    }
}