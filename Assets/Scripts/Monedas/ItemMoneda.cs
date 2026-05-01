using UnityEngine;
using UnityEngine.UIElements;

public class ItemMoneda : MonoBehaviour
{
    // Se ejecuta cuando otro collider entra en contacto con la moneda
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Sumar la moneda al saldo del jugador
            MonedaManager.instance.Sumar();

            // Actualizar el contador de monedas en la UI
            var doc   = FindFirstObjectByType<UIDocument>();
            var root  = doc?.rootVisualElement;
            var label = root?.Q<Label>("NumeroMonedas");

            if (label != null)
                label.text = MonedaManager.instance.totalMonedas.ToString();

            // Ocultar el sprite y destruir el objeto tras una breve pausa
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}