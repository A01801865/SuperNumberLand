using UnityEngine;
using TMPro;

public class ObjetoRespuesta : MonoBehaviour
{
    public int valor;
    private TextMeshPro textoTMP;

    private static int rachaCorrectas = 0;
    private static float tiempoUltimaPregunta = -1f;

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

        // Registrar el momento en que apareció la pregunta
        tiempoUltimaPregunta = Time.time;
    }

    public void RecibirGolpe()
    {
        GeneradorPreguntas generador = FindFirstObjectByType<GeneradorPreguntas>();
        if (generador == null) return;

        UIPreguntaController ui = FindFirstObjectByType<UIPreguntaController>();

        if (valor == generador.RespuestaCorrecta)
        {
            Debug.Log("Correcto");

            // Logro: Dedo Veloz (idBD 8) — respondió en menos de 3 segundos
            if (tiempoUltimaPregunta >= 0 && (Time.time - tiempoUltimaPregunta) < 3f)
                LogrosManager.Instance?.DesbloquearLogro(8);

            // Racha para Intocable y Cerebro Brillante
            rachaCorrectas++;
            if (rachaCorrectas >= 10)
            {
                LogrosManager.Instance?.DesbloquearLogro(2);  // Intocable
                LogrosManager.Instance?.DesbloquearLogro(3);  // Cerebro Brillante
            }

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

            // Resetear racha al fallar
            rachaCorrectas = 0;

            if (ui != null)
                ui.MostrarIncorrecto();

            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player != null)
                player.PerderVida();
        }
    }

    public static void ResetearRacha()
    {
        rachaCorrectas = 0;
        tiempoUltimaPregunta = -1f;
    }
}