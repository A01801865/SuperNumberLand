using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BotonJugarLobby : MonoBehaviour
{
    private UIDocument menu;
    private Button botonJugar;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonJugar = root.Q<Button>("BotonJugar");

        if (botonJugar != null)
        {
            botonJugar.RegisterCallback<ClickEvent>(IrALobby);
        }
    }

    void OnDisable()
    {
        if (botonJugar != null)
        {
            botonJugar.UnregisterCallback<ClickEvent>(IrALobby);
        }
    }

    void IrALobby(ClickEvent evt)
    {
        SceneManager.LoadScene("Lobby");
    }
}