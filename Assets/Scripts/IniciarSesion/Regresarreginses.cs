using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Regresareginses : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRegresar;

    void OnEnable()
    {
        // Obtener el botón y suscribirlo al método para regresar a la pantalla de registro/inicio
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonRegresar = root.Q<Button>("BotonVolver");
        if (botonRegresar != null)
            botonRegresar.clicked += CerrarEscena;
    }

    void OnDisable()
    {
        // Desuscribir el botón al deshabilitar el objeto para evitar llamadas duplicadas
        if (botonRegresar != null)
            botonRegresar.clicked -= CerrarEscena;
    }

    // Regresa a la pantalla de registro e inicio de sesión
    void CerrarEscena()
    {
        SceneManager.LoadScene("Reg_InSes");
    }
}