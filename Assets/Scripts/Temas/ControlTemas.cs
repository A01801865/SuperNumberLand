using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ControlTemas : MonoBehaviour
{
    private Button suma;
    private Button resta;
    private Button multiplicacion;
    private Button division;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ⚠️ estos names deben coincidir con UI Builder
        suma = root.Q<Button>("suma");
        resta = root.Q<Button>("resta");
        multiplicacion = root.Q<Button>("multiplicacion");
        division = root.Q<Button>("division");

        int edad = PlayerPrefs.GetInt("edad");
        Debug.Log("Edad jugador: " + edad);

        ConfigurarTemas(edad);
        ConfigurarBotones();
    }

    void ConfigurarTemas(int edad)
    {
        // Siempre activos
        suma.SetEnabled(true);
        resta.SetEnabled(true);

        // Multiplicación
        if (edad >= 8)
        {
            multiplicacion.SetEnabled(true);
            multiplicacion.style.opacity = 1f;
        }
        else
        {
            multiplicacion.SetEnabled(false);
            multiplicacion.style.opacity = 0.5f;
        }

        // División
        if (edad >= 10)
        {
            division.SetEnabled(true);
            division.style.opacity = 1f;
        }
        else
        {
            division.SetEnabled(false);
            division.style.opacity = 0.5f;
        }
    }

    void ConfigurarBotones()
    {
        suma.clicked += () => CargarNivel("Niveles");
        resta.clicked += () => CargarNivel("NivelesResta");
        multiplicacion.clicked += () => CargarNivel("NivelesMulti");
        division.clicked += () => CargarNivel("NivelesDivi");
    }

    void CargarNivel(string escena)
    {
        Debug.Log("Cargando: " + escena);
        SceneManager.LoadScene(escena);
    }
}