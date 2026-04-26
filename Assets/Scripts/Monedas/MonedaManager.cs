using UnityEngine;

public class MonedaManager : MonoBehaviour
{
    public static MonedaManager instance;

    public int totalMonedas = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Sumar(int cantidad = 1)
    {
        totalMonedas += cantidad;
    }
}