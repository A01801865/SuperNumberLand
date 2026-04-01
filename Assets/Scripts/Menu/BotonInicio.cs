using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BotonInicio : MonoBehaviour
{
    private UIDocument menu;
    private Button botonPlay;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonPlay = root.Q<Button>("BotonPlay");

        //Callbacks (1)
        botonPlay.RegisterCallback<ClickEvent>(IrARegistro);
    }

    void OnDisable()
    {
        botonPlay.UnregisterCallback<ClickEvent>(IrARegistro);
    }

    void IrARegistro(ClickEvent evt)
    {
        SceneManager.LoadScene("Reg_InSes");
    }
}