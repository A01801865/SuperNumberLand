using UnityEngine;
using TMPro;

public class ObjetoRespuesta : MonoBehaviour
{
    public int valor;
    private TextMeshPro textoTMP;

    void Start()
    {
        textoTMP = GetComponentInChildren<TextMeshPro>();
    }

    public void SetValor(int nuevoValor)
    {
        valor = nuevoValor;

        if (textoTMP == null)
            textoTMP = GetComponentInChildren<TextMeshPro>();

        if (textoTMP != null)
            textoTMP.text = valor.ToString();
    }

    public void RecibirGolpe()
    {
        GeneradorPreguntas generador = FindFirstObjectByType<GeneradorPreguntas>();
        if (generador == null) return;

        UIPreguntaController ui = FindFirstObjectByType<UIPreguntaController>();

        if (valor == generador.RespuestaCorrecta)
        {
            Debug.Log("Correcto");

            Puerta puerta = FindFirstObjectByType<Puerta>();
            if (puerta != null)
                puerta.Abrir();

            SpawnerRespuestas spawner = FindFirstObjectByType<SpawnerRespuestas>();
            if (spawner != null)
                foreach (Transform hijo in spawner.transform)
                    Destroy(hijo.gameObject);

            if (ui != null)
                ui.MostrarPuertaAbierta();
        }
        else
        {
            Debug.Log("Incorrecto");

            if (ui != null)
                ui.MostrarIncorrecto();

            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player != null)
                player.PerderVida();
        }
    }
}