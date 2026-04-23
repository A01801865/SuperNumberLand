using UnityEngine;
using UnityEngine.UIElements;

public class UIVidasToolkit : MonoBehaviour
{
    private VisualElement vida1;
    private VisualElement vida2;
    private VisualElement vida3;

    private VisualElement pantallaPerder; // 👈 FondoPerder

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Corazones
        vida1 = root.Q<VisualElement>("Vida_1");
        vida2 = root.Q<VisualElement>("Vida_2");
        vida3 = root.Q<VisualElement>("Vida_3");

        // 💀 Pantalla de perder (IMPORTANTE)
        pantallaPerder = root.Q<VisualElement>("FondoPerder");

        // Debug para confirmar
        Debug.Log("vida1: " + vida1);
        Debug.Log("vida2: " + vida2);
        Debug.Log("vida3: " + vida3);
        Debug.Log("pantallaPerder: " + pantallaPerder);

        // Ocultar al inicio
        if (pantallaPerder != null)
            pantallaPerder.style.display = DisplayStyle.None;
    }

    public void ActualizarVidas(int vidas)
    {
        if (vida1 != null)
            vida1.style.display = vidas >= 1 ? DisplayStyle.Flex : DisplayStyle.None;

        if (vida2 != null)
            vida2.style.display = vidas >= 2 ? DisplayStyle.Flex : DisplayStyle.None;

        if (vida3 != null)
            vida3.style.display = vidas >= 3 ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void MostrarPantallaPerder()
    {
        Debug.Log("MOSTRANDO PANTALLA DE PERDER");

        if (pantallaPerder != null)
        {
            pantallaPerder.style.display = DisplayStyle.Flex;
        }
        else
        {
            Debug.LogError("No se encontró FondoPerder");
        }
    }
}