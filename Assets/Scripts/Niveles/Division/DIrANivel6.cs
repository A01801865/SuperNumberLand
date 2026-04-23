using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel6 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDSeis;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonDSeis = root.Q<Button>("DSeis");

        if (botonDSeis != null)
        {
            botonDSeis.clicked += AbrirDNivel6;
        }
    }

    void OnDisable()
    {
        if (botonDSeis != null)
        {
            botonDSeis.clicked -= AbrirDNivel6;
        }
    }

    void AbrirDNivel6()
    {
        SceneManager.LoadScene("DNivel6");
    }
}