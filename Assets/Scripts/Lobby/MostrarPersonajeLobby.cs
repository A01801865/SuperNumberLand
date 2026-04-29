using UnityEngine;

public class MostrarPersonajeLobby : MonoBehaviour
{
    public GameObject[] personajes;
    public Transform puntoSpawn;

    private GameObject personajeActual;

    void Start()
    {
        // No spawneamos aqui — LobbyController lo hara despues de cargar la seleccion
    }

    void Update()
    {
        if (personajeActual == null)
            Debug.LogWarning("personajeActual es null");
    }

    public void MostrarPersonaje(int index = -1)
    {
        if (personajeActual != null)
            Destroy(personajeActual);

        if (index == -1)
        {
            index = 0;
            if (GameManager.instancia != null)
                index = GameManager.instancia.personajeSeleccionado;
        }

        Debug.Log("MostrarPersonaje llamado con index: " + index);

        if (personajes.Length > index)
        {
            personajeActual = Instantiate(personajes[index], puntoSpawn.position, Quaternion.identity);
            personajeActual.transform.localScale = new Vector3(20f, 20f, 18f);

            var pc = personajeActual.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;

            var rb = personajeActual.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;
        }
    }
}