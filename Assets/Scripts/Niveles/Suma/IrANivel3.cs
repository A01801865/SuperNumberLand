using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel3 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonTres;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonTres = root.Q<Button>("Tres");
        if (botonTres != null)
            botonTres.clicked += AbrirNivel3;
    }

    void OnDisable()
    {
        if (botonTres != null)
            botonTres.clicked -= AbrirNivel3;
    }

    void AbrirNivel3()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Mapa3");
    }
}