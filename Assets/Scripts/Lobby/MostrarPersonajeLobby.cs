using UnityEngine;

public class MostrarPersonajeLobby : MonoBehaviour
{
    public GameObject[] personajes;
    public Transform puntoSpawn;

    private GameObject personajeActual;

    void Start()
    {
        MostrarPersonaje();
    }

    public void MostrarPersonaje()
    {
        if (personajeActual != null)
            Destroy(personajeActual);

        int index = 0;
        if (GameManager.instancia != null)
            index = GameManager.instancia.personajeSeleccionado;

        if (personajes.Length > index)
        {
            personajeActual = Instantiate(personajes[index], puntoSpawn.position, Quaternion.identity);

            personajeActual.transform.localScale = new Vector3(20f, 20f, 18f); // ← Ajusta a tu gusto

            var pc = personajeActual.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;

            var rb = personajeActual.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;
        }
    }
}