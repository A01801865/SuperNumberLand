using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DIrANivel4 : MonoBehaviour
{
    private UIDocument menu;
    private Button botonDCuatro;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        var root = menu.rootVisualElement;

        botonDCuatro = root.Q<Button>("DCuatro");

        if (botonDCuatro != null)
        {
            botonDCuatro.clicked += AbrirDNivel4;
        }
    }

    void OnDisable()
    {
        if (botonDCuatro != null)
        {
            botonDCuatro.clicked -= AbrirDNivel4;
        }
    }

    void AbrirDNivel4()
    {
        SceneManager.LoadScene("DNivel4");
    }
}