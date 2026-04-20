using UnityEngine;
using UnityEngine.UIElements;

public class UIVidasToolkit : MonoBehaviour
{
    private VisualElement vida1;
    private VisualElement vida2;
    private VisualElement vida3;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        vida1 = root.Q<VisualElement>("Vida_1");
        vida2 = root.Q<VisualElement>("Vida_2");
        vida3 = root.Q<VisualElement>("Vida_3");
    }

    public void ActualizarVidas(int vidas)
    {
        vida1.style.display = vidas >= 1 ? DisplayStyle.Flex : DisplayStyle.None;
        vida2.style.display = vidas >= 2 ? DisplayStyle.Flex : DisplayStyle.None;
        vida3.style.display = vidas >= 3 ? DisplayStyle.Flex : DisplayStyle.None;
    }
}