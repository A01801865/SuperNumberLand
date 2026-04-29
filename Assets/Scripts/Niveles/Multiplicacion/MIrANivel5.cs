using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel5 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMCinco;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonMCinco = root.Q<Button>("MCinco");
        if (botonMCinco != null)
            botonMCinco.clicked += AbrirMNivel5;
    }

    void OnDisable()
    {
        if (botonMCinco != null)
            botonMCinco.clicked -= AbrirMNivel5;
    }

    void AbrirMNivel5()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 5);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MNivel5");
    }
}