using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MIrANivel1 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonMUno;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonMUno = root.Q<Button>("MUno");
        if (botonMUno != null)
            botonMUno.clicked += AbrirMNivel1;
    }

    void OnDisable()
    {
        if (botonMUno != null)
            botonMUno.clicked -= AbrirMNivel1;
    }

    void AbrirMNivel1()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MNivel1");
    }
}