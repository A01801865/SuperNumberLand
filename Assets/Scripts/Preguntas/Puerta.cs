using UnityEngine;

public class Puerta : MonoBehaviour
{
    public void Abrir()
    {
        Debug.Log("Puerta abierta");
        gameObject.SetActive(false); // desaparece
    }
}