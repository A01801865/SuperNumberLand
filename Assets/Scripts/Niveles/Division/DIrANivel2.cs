using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel2 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDDos;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonDDos = root.Q<Button>("DDos");

        if (botonDDos != null)
        {
            botonDDos.clicked += AbrirDNivel2;
        }
    }

    void OnDisable()
    {
        if (botonDDos != null)
        {
            botonDDos.clicked -= AbrirDNivel2;
        }
    }

    void AbrirDNivel2()
    {
        SceneManager.LoadScene("DNivel2");
    }
}