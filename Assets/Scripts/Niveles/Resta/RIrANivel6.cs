using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel6 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRSeis;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonRSeis = root.Q<Button>("RSeis");
        if (botonRSeis != null)
            botonRSeis.clicked += AbrirRNivel6;
    }

    void OnDisable()
    {
        if (botonRSeis != null)
            botonRSeis.clicked -= AbrirRNivel6;
    }

    void AbrirRNivel6()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 6);
        PlayerPrefs.Save();
        SceneManager.LoadScene("RNivel6");
    }
}