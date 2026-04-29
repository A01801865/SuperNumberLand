using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel4 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMCuatro;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonMCuatro = root.Q<Button>("MCuatro");
        if (botonMCuatro != null)
            botonMCuatro.clicked += AbrirMNivel4;
    }

    void OnDisable()
    {
        if (botonMCuatro != null)
            botonMCuatro.clicked -= AbrirMNivel4;
    }

    void AbrirMNivel4()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 4);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MNivel4");
    }
}