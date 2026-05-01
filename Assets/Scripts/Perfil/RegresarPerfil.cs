using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RegresarPerfil : MonoBehaviour
{
    private UIDocument menu;
    private Button boton;

    void OnEnable()
    {
        // Obtener el botón y suscribirlo al método para regresar al Lobby
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        boton = root.Q<Button>("BotonVolver");
        if (boton != null)
            boton.clicked += Volver;
    }

    void OnDisable()
    {
        // Desuscribir el botón al deshabilitar el objeto para evitar llamadas duplicadas
        if (boton != null)
            boton.clicked -= Volver;
    }

    // Regresa al Lobby desde la pantalla de perfil
    void Volver()
    {
        SceneManager.LoadScene("Lobby");
    }
}