using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel10 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMDiez;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonMDiez = root.Q<Button>("MDiez");
        if (botonMDiez != null)
            botonMDiez.clicked += AbrirMNivel10;
    }

    void OnDisable()
    {
        if (botonMDiez != null)
            botonMDiez.clicked -= AbrirMNivel10;
    }

    void AbrirMNivel10()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 10);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MNivel10");
    }
}