using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel5 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRCinco;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonRCinco = root.Q<Button>("RCinco");
        if (botonRCinco != null)
            botonRCinco.clicked += AbrirRNivel5;
    }

    void OnDisable()
    {
        if (botonRCinco != null)
            botonRCinco.clicked -= AbrirRNivel5;
    }

    void AbrirRNivel5()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 5);
        PlayerPrefs.Save();
        SceneManager.LoadScene("RNivel5");
    }
}