using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel7 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDSiete;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonDSiete = root.Q<Button>("DSiete");
        if (botonDSiete != null)
            botonDSiete.clicked += AbrirDNivel7;
    }

    void OnDisable()
    {
        if (botonDSiete != null)
            botonDSiete.clicked -= AbrirDNivel7;
    }

    void AbrirDNivel7()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 7);
        PlayerPrefs.Save();
        SceneManager.LoadScene("DNivel7");
    }
}