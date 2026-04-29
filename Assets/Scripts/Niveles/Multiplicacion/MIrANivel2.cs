using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel2 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMDos;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonMDos = root.Q<Button>("MDos");
        if (botonMDos != null)
            botonMDos.clicked += AbrirMNivel2;
    }

    void OnDisable()
    {
        if (botonMDos != null)
            botonMDos.clicked -= AbrirMNivel2;
    }

    void AbrirMNivel2()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MNivel2");
    }
}