using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel7 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMSiete;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonMSiete = root.Q<Button>("MSiete");
        if (botonMSiete != null)
            botonMSiete.clicked += AbrirMNivel7;
    }

    void OnDisable()
    {
        if (botonMSiete != null)
            botonMSiete.clicked -= AbrirMNivel7;
    }

    void AbrirMNivel7()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 7);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MNivel7");
    }
}