using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel6 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonSeis;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonSeis = root.Q<Button>("Seis");
        if (botonSeis != null)
            botonSeis.clicked += AbrirNivel6;
    }

    void OnDisable()
    {
        if (botonSeis != null)
            botonSeis.clicked -= AbrirNivel6;
    }

    void AbrirNivel6()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 6);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Mapa6");
    }
}