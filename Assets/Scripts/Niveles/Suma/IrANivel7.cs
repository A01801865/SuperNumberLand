using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel7 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonSiete;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonSiete = root.Q<Button>("Siete");
        if (botonSiete != null)
            botonSiete.clicked += AbrirNivel7;
    }

    void OnDisable()
    {
        if (botonSiete != null)
            botonSiete.clicked -= AbrirNivel7;
    }

    void AbrirNivel7()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 7);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Mapa7");
    }
}