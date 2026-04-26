using UnityEngine;
using UnityEngine.UIElements;

public class ItemMoneda : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MonedaManager.instance.Sumar();

            var doc = FindFirstObjectByType<UIDocument>();
            var root = doc?.rootVisualElement;
            var label = root?.Q<Label>("NumeroMonedas");

            if (label != null)
                label.text = MonedaManager.instance.totalMonedas.ToString();

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}