using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel3 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRTres;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonRTres = root.Q<Button>("RTres");
        if (botonRTres != null)
            botonRTres.clicked += AbrirRNivel3;
    }

    void OnDisable()
    {
        if (botonRTres != null)
            botonRTres.clicked -= AbrirRNivel3;
    }

    void AbrirRNivel3()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene("RNivel3");
    }
}