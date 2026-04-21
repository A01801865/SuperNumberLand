using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIResultadoController : MonoBehaviour
{
    private Button btnReintentar;
    private Button btnVolver;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

       
        btnReintentar = root.Q<Button>("BotonReintentar");
        btnVolver = root.Q<Button>("BotonVolver");

        btnReintentar.clicked += Reintentar;
        btnVolver.clicked += VolverMenu;
    }

    void Reintentar()
    {
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name);
    }

    void VolverMenu()
    {
        SceneManager.LoadScene("Niveles"); 
    }
}