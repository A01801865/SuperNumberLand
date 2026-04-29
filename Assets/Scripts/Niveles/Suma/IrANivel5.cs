using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel5 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonCinco;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonCinco = root.Q<Button>("Cinco");
        if (botonCinco != null)
            botonCinco.clicked += AbrirNivel5;
    }

    void OnDisable()
    {
        if (botonCinco != null)
            botonCinco.clicked -= AbrirNivel5;
    }

    void AbrirNivel5()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 5);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Mapa5");
    }
}