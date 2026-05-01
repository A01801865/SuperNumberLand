using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Creditos : MonoBehaviour
{
    private UIDocument creditos;
    private Button boton;

    void OnEnable()
    {
        // Obtener el botón de regreso desde la UI y suscribirlo al método Volver
        creditos = GetComponent<UIDocument>();
        var root = creditos.rootVisualElement;

        boton = root.Q<Button>("BotonVolve");
        if (boton != null)
            boton.clicked += Volver;
    }

    void OnDisable()
    {
        // Desuscribir el botón al deshabilitar el objeto para evitar llamadas duplicadas
        if (boton != null)
            boton.clicked -= Volver;
    }

    // Regresa al Lobby al presionar el botón
    void Volver()
    {
        SceneManager.LoadScene("Lobby");
    }
}