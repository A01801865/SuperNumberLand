using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrAPerfil : MonoBehaviour
{
    private UIDocument menu;
    private Button boton;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        boton = root.Q<Button>("Perfil");
        if (boton != null)
        {
            boton.clicked += Abrir;
        }
    }

    void OnDisable()
    {
        if (boton != null)
        {
            boton.clicked -= Abrir;
        }
    }

    void Abrir()
    {
        SceneManager.LoadScene("Perfil");
    }
}