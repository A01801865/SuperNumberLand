using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel9 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDNueve;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonDNueve = root.Q<Button>("DNueve");
        if (botonDNueve != null)
            botonDNueve.clicked += AbrirDNivel9;
    }

    void OnDisable()
    {
        if (botonDNueve != null)
            botonDNueve.clicked -= AbrirDNivel9;
    }

    void AbrirDNivel9()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 9);
        PlayerPrefs.Save();
        SceneManager.LoadScene("DNivel9");
    }
}