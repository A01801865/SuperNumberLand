    using UnityEngine;

public class SpawnerAnimales : MonoBehaviour
{
    public GameObject[] animales;

    public float tiempoMin = 4f;
    public float tiempoMax = 7f;

    public float posY = -3.1f; 

    public float limiteIzq = -10f;
    public float limiteDer = 10f;

    public int maxAnimales = 3;
    public int actuales = 0;

    void Start()
    {
        Invoke("SpawnAnimal", Random.Range(tiempoMin, tiempoMax));
    }

    void SpawnAnimal()
    {
        if (actuales >= maxAnimales)
        {
            Invoke("SpawnAnimal", Random.Range(tiempoMin, tiempoMax));
            return;
        }

        // Elegir animal random
        GameObject prefab = animales[Random.Range(0, animales.Length)];
        GameObject animal = Instantiate(prefab);

        // 🔥 Ajuste automático al piso
        SpriteRenderer sr = animal.GetComponent<SpriteRenderer>();
        float altura = sr.bounds.extents.y;
        float y = posY + altura;

        // Lado random
        bool desdeIzquierda = Random.value > 0.5f;

        if (desdeIzquierda)
        {
            animal.transform.position = new Vector2(limiteIzq, y);
        }
        else
        {
            animal.transform.position = new Vector2(limiteDer, y);

            // Voltear
            Vector3 escala = animal.transform.localScale;
            escala.x *= -1;
            animal.transform.localScale = escala;
        }

        // Tamaño random
        float escalaRandom = Random.Range(0.8f, 1.2f);
        animal.transform.localScale *= escalaRandom;

        actuales++;

        // Volver a spawnear
        Invoke("SpawnAnimal", Random.Range(tiempoMin, tiempoMax));
    }
}