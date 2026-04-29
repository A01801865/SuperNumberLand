using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IrANivel1 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonUno;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonUno = root.Q<Button>("Uno");
        if (botonUno != null)
            botonUno.clicked += AbrirNivel1;
    }

    void OnDisable()
    {
        if (botonUno != null)
            botonUno.clicked -= AbrirNivel1;
    }

    void AbrirNivel1()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Nivel1");
    }
}