using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel8 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonOcho;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonOcho = root.Q<Button>("Ocho");
        if (botonOcho != null)
            botonOcho.clicked += AbrirNivel8;
    }

    void OnDisable()
    {
        if (botonOcho != null)
            botonOcho.clicked -= AbrirNivel8;
    }

    void AbrirNivel8()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 8);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Mapa8");
    }
}