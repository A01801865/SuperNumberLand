using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrAOlviCont : MonoBehaviour
{
    private UIDocument menu;
    private Button botonOlvidar;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonOlvidar = root.Q<Button>("BotonOlCont");
        if (botonOlvidar != null)
        {
            botonOlvidar.clicked += AbrirOlvido;
        }
    }

    void OnDisable()
    {
        if (botonOlvidar != null)
        {
            botonOlvidar.clicked -= AbrirOlvido;
        }
    }

    void AbrirOlvido()
    {
        SceneManager.LoadScene("OlviCont");
    }
}