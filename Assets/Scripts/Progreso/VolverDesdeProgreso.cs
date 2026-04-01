using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class VolverDesdeProgreso : MonoBehaviour
{
    private UIDocument menu;
    private Button boton;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        boton = root.Q<Button>("BotonVolver");
        if (boton != null)
        {
            boton.clicked += Volver;
        }
    }

    void OnDisable()
    {
        if (boton != null)
        {
            boton.clicked -= Volver;
        }
    }

    void Volver()
    {
        SceneManager.LoadScene("Lobby");
    }
}