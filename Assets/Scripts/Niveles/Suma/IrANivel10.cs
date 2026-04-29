using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel10 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDiez;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonDiez = root.Q<Button>("Diez");
        if (botonDiez != null)
            botonDiez.clicked += AbrirNivel10;
    }

    void OnDisable()
    {
        if (botonDiez != null)
            botonDiez.clicked -= AbrirNivel10;
    }

    void AbrirNivel10()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 10);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Mapa10");
    }
}