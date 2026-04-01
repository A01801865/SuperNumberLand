using UnityEngine;
using UnityEngine.UIElements;

public class TiendaController : MonoBehaviour
{
    private VisualElement personajes;
    private VisualElement fondos;

    private Button botonPersonajes;
    private Button botonFondos;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        personajes = root.Q<VisualElement>("Personajes");
        fondos = root.Q<VisualElement>("Fondos");

        botonPersonajes = root.Q<Button>("BotonPersonajes");
        botonFondos = root.Q<Button>("BotonFondos");

        personajes.style.display = DisplayStyle.Flex;
        fondos.style.display = DisplayStyle.None;

        botonPersonajes.clicked += MostrarPersonajes;
        botonFondos.clicked += MostrarFondos;
    }

    void OnDisable()
    {
        botonPersonajes.clicked -= MostrarPersonajes;
        botonFondos.clicked -= MostrarFondos;
    }

    private void MostrarPersonajes()
    {
        personajes.style.display = DisplayStyle.Flex;
        fondos.style.display = DisplayStyle.None;
    }

    private void MostrarFondos()
    {
        personajes.style.display = DisplayStyle.None;
        fondos.style.display = DisplayStyle.Flex;
    }
}