using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrAOlviCont : MonoBehaviour
{
    private UIDocument menu;
    private Button botonOlvidar;

    void OnEnable()
    {
        // Obtener el botón y suscribirlo al método para ir a la pantalla de contraseña olvidada
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonOlvidar = root.Q<Button>("BotonOlCont");
        if (botonOlvidar != null)
            botonOlvidar.clicked += AbrirOlvido;
    }

    void OnDisable()
    {
        // Desuscribir el botón al deshabilitar el objeto para evitar llamadas duplicadas
        if (botonOlvidar != null)
            botonOlvidar.clicked -= AbrirOlvido;
    }

    // Carga la escena de recuperación de contraseña
    void AbrirOlvido()
    {
        SceneManager.LoadScene("OlviCont");
    }
}