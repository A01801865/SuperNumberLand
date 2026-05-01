using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrAPerfil : MonoBehaviour
{
    private UIDocument menu;
    private Button boton;

    void OnEnable()
    {
        // Obtener el botón y suscribirlo al método para abrir la pantalla de perfil
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        boton = root.Q<Button>("Perfil");
        if (boton != null)
            boton.clicked += Abrir;
    }

    void OnDisable()
    {
        // Desuscribir el botón al deshabilitar el objeto para evitar llamadas duplicadas
        if (boton != null)
            boton.clicked -= Abrir;
    }

    // Carga la escena del perfil del jugador
    void Abrir()
    {
        SceneManager.LoadScene("Perfil");
    }
}