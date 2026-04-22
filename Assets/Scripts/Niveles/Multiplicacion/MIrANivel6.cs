using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel6 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMSeis;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonMSeis = root.Q<Button>("MSeis");

        if (botonMSeis != null)
        {
            botonMSeis.clicked += AbrirMNivel6;
        }
    }

    void OnDisable()
    {
        if (botonMSeis != null)
        {
            botonMSeis.clicked -= AbrirMNivel6;
        }
    }

    void AbrirMNivel6()
    {
        SceneManager.LoadScene("MNivel6");
    }
}