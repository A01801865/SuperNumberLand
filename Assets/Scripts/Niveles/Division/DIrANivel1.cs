using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel1 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDUno;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonDUno = root.Q<Button>("DUno");

        if (botonDUno != null)
        {
            botonDUno.clicked += AbrirDNivel1;
        }
    }

    void OnDisable()
    {
        if (botonDUno != null)
        {
            botonDUno.clicked -= AbrirDNivel1;
        }
    }

    void AbrirDNivel1()
    {
        SceneManager.LoadScene("DNivel1");
    }
}