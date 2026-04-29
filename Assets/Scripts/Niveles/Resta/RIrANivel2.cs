using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RIrANivel2 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonRDos;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;
        botonRDos = root.Q<Button>("RDos");
        if (botonRDos != null)
            botonRDos.clicked += AbrirRNivel2;
    }

    void OnDisable()
    {
        if (botonRDos != null)
            botonRDos.clicked -= AbrirRNivel2;
    }

    void AbrirRNivel2()
    {
        GameManagerProgreso.Instance?.ResetearMapas();
        PlayerPrefs.SetInt("nivel_seleccionado", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene("RNivel2");
    }
}