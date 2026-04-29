using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel7 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRSiete;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonRSiete = root.Q<Button>("RSiete");
        if (botonRSiete != null)
            botonRSiete.clicked += AbrirRNivel7;
    }

    void OnDisable()
    {
        if (botonRSiete != null)
            botonRSiete.clicked -= AbrirRNivel7;
    }

    void AbrirRNivel7()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 7);
        PlayerPrefs.Save();
        SceneManager.LoadScene("RNivel7");
    }
}