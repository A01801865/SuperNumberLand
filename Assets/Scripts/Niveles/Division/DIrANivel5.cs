using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel5 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDCinco;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonDCinco = root.Q<Button>("DCinco");

        if (botonDCinco != null)
        {
            botonDCinco.clicked += AbrirDNivel5;
        }
    }

    void OnDisable()
    {
        if (botonDCinco != null)
        {
            botonDCinco.clicked -= AbrirDNivel5;
        }
    }

    void AbrirDNivel5()
    {
        SceneManager.LoadScene("DNivel5");
    }
}