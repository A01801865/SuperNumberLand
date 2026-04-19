using UnityEngine;

public class AnimalMovimiento : MonoBehaviour
{
    public float velocidadMin = 1.5f;
    public float velocidadMax = 3f;

    private float velocidad;
    private int direccion = 1;

    void Start()
    {
        velocidad = Random.Range(velocidadMin, velocidadMax);

        if (transform.localScale.x < 0)
            direccion = -1;
    }

    void Update()
    {
        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);

        if (transform.position.x > 12 || transform.position.x < -12)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SpawnerAnimales spawner = FindObjectOfType<SpawnerAnimales>();
        if (spawner != null)
        {
            spawner.actuales--;
        }
    }
}