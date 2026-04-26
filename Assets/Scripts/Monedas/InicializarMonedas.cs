using UnityEngine;
using UnityEngine.UIElements;

public class InicializarMonedas : MonoBehaviour
{
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var label = root?.Q<Label>("NumeroMonedas");

        if (label != null)
            label.text = MonedaManager.instance.totalMonedas.ToString();
    }
}