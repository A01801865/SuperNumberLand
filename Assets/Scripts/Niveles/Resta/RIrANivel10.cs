using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel10 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRDiez;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonRDiez = root.Q<Button>("RDiez");
        if (botonRDiez != null)
            botonRDiez.clicked += AbrirRNivel10;
    }

    void OnDisable()
    {
        if (botonRDiez != null)
            botonRDiez.clicked -= AbrirRNivel10;
    }

    void AbrirRNivel10()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 10);
        PlayerPrefs.Save();
        SceneManager.LoadScene("RNivel10");
    }
}