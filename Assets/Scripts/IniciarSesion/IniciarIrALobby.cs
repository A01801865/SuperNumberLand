using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IniciarIrALobby : MonoBehaviour
{
    private UIDocument menu;
    private Button botonIniciarSesion;

    void OnEnable()
    {
        // Obtener el botón de inicio de sesión y suscribirlo al método AbrirLobby
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonIniciarSesion = root.Q<Button>("BotonIniciarSes");
        if (botonIniciarSesion != null)
            botonIniciarSesion.clicked += AbrirLobby;
    }

    void OnDisable()
    {
        // Desuscribir el botón al deshabilitar el objeto para evitar llamadas duplicadas
        if (botonIniciarSesion != null)
            botonIniciarSesion.clicked -= AbrirLobby;
    }

    // Carga la escena del Lobby al presionar el botón
    void AbrirLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}