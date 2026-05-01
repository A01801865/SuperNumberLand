using UnityEngine;
using UnityEngine.UIElements;

public class InicializarMonedas : MonoBehaviour
{
    void Start()
    {
        // Buscar el label de monedas en la UI y mostrar el saldo actual del jugador
        var root  = GetComponent<UIDocument>().rootVisualElement;
        var label = root?.Q<Label>("NumeroMonedas");

        if (label != null)
            label.text = MonedaManager.instance.totalMonedas.ToString();
    }
}