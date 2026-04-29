using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel4 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRCuatro;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonRCuatro = root.Q<Button>("RCuatro");
        if (botonRCuatro != null)
            botonRCuatro.clicked += AbrirRNivel4;
    }

    void OnDisable()
    {
        if (botonRCuatro != null)
            botonRCuatro.clicked -= AbrirRNivel4;
    }

    void AbrirRNivel4()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 4);
        PlayerPrefs.Save();
        SceneManager.LoadScene("RNivel4");
    }
}