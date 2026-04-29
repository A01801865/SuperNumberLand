using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel8 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMOcho;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonMOcho = root.Q<Button>("MOcho");
        if (botonMOcho != null)
            botonMOcho.clicked += AbrirMNivel8;
    }

    void OnDisable()
    {
        if (botonMOcho != null)
            botonMOcho.clicked -= AbrirMNivel8;
    }

    void AbrirMNivel8()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 8);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MNivel8");
    }
}