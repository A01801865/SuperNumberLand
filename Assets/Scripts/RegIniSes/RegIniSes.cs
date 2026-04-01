using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RegIniSes : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRegistro;
    private Button botonIniciarSesion;
    private Button botonVolver;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonRegistro = root.Q<Button>("BotonRegistro");
        botonIniciarSesion = root.Q<Button>("BotonInSes");
        botonVolver = root.Q<Button>("BotonVolver");

        //Callbacks
        botonRegistro.RegisterCallback<ClickEvent>(IrARegistro);
        botonIniciarSesion.RegisterCallback<ClickEvent>(IrAIniciarSesion);
        botonVolver.RegisterCallback<ClickEvent>(IrAMenu);
    }

    void OnDisable()
    {
        botonRegistro.UnregisterCallback<ClickEvent>(IrARegistro);
        botonIniciarSesion.UnregisterCallback<ClickEvent>(IrAIniciarSesion);
        botonVolver.UnregisterCallback<ClickEvent>(IrAMenu);
    }

    void IrARegistro(ClickEvent evt)
    {
        SceneManager.LoadScene("Registro");
    }

    void IrAIniciarSesion(ClickEvent evt)
    {
        SceneManager.LoadScene("IniciarSesion");
    }

    void IrAMenu(ClickEvent evt)
    {
        SceneManager.LoadScene("Menu");
    }
}