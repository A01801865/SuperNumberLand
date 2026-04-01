using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IniciarIrALobby : MonoBehaviour
{
    private UIDocument menu;
    private Button botonIniciarSesion;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonIniciarSesion = root.Q<Button>("BotonIniciarSes");
        if (botonIniciarSesion != null)
        {
            botonIniciarSesion.clicked += AbrirLobby;
        }
    }

    void OnDisable()
    {
        if (botonIniciarSesion != null)
        {
            botonIniciarSesion.clicked -= AbrirLobby;
        }
    }

    void AbrirLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}