using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Regresareginses : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRegresar;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonRegresar = root.Q<Button>("BotonVolver");
        if (botonRegresar != null)
        {
            botonRegresar.clicked += CerrarEscena;
        }
    }

    void OnDisable()
    {
        if (botonRegresar != null)
        {
            botonRegresar.clicked -= CerrarEscena;
        }
    }

    void CerrarEscena()
    {
        SceneManager.LoadScene("Reg_InSes");
    }
}