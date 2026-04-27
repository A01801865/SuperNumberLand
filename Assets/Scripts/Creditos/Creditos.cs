using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Creditos : MonoBehaviour
{
    private UIDocument creditos;
    private Button boton;

    void OnEnable()
    {
        creditos = GetComponent<UIDocument>();
        var root = creditos.rootVisualElement;

        boton = root.Q<Button>("BotonVolve");
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