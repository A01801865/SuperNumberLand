using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel8 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonROcho;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonROcho = root.Q<Button>("ROcho");
        if (botonROcho != null)
            botonROcho.clicked += AbrirRNivel8;
    }

    void OnDisable()
    {
        if (botonROcho != null)
            botonROcho.clicked -= AbrirRNivel8;
    }

    void AbrirRNivel8()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 8);
        PlayerPrefs.Save();
        SceneManager.LoadScene("RNivel8");
    }
}