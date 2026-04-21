using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel1 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRUno;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonRUno = root.Q<Button>("RUno");

        if (botonRUno != null)
        {
            botonRUno.clicked += AbrirRNivel1;
        }
    }

    void OnDisable()
    {
        if (botonRUno != null)
        {
            botonRUno.clicked -= AbrirRNivel1;
        }
    }

    void AbrirRNivel1()
    {
        SceneManager.LoadScene("RNivel1");
    }
}