using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrATemas : MonoBehaviour
{
    private UIDocument menu;
    private Button boton;

    void OnEnable()
    {
        // Obtener el botón "Jugar" y suscribirlo al método para abrir la selección de temas
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        boton = root.Q<Button>("Jugar");
        if (boton != null)
            boton.clicked += Abrir;
    }

    void OnDisable()
    {
        // Desuscribir el botón al deshabilitar el objeto para evitar llamadas duplicadas
        if (boton != null)
            boton.clicked -= Abrir;
    }

    // Carga la escena de selección de temas
    void Abrir()
    {
        SceneManager.LoadScene("Temas");
    }
}