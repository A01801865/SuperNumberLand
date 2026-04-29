using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RVolverANiveles : MonoBehaviour
{
    private UIDocument menu;
    private Button botonVolver;

    void Start()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonVolver = root.Q<Button>("BotonVolver");

        if (botonVolver != null)
        {
            Debug.Log("RVolverANiveles: BotonVolver encontrado ✅");
            botonVolver.clicked += Volver;
        }
        else
        {
            Debug.LogError("RVolverANiveles: BotonVolver NO encontrado ❌");
        }
    }

    void Volver()
    {
        Debug.Log("RVolverANiveles: Volver() ejecutado → NivelesResta");
        SceneManager.LoadScene("NivelesResta");
    }
}