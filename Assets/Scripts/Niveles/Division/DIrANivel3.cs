using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel3 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDTres;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonDTres = root.Q<Button>("DTres");
        if (botonDTres != null)
            botonDTres.clicked += AbrirDNivel3;
    }

    void OnDisable()
    {
        if (botonDTres != null)
            botonDTres.clicked -= AbrirDNivel3;
    }

    void AbrirDNivel3()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene("DNivel3");
    }
}