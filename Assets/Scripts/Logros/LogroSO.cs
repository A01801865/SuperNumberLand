using UnityEngine;

[CreateAssetMenu(fileName = "NuevoLogro", menuName = "Sistema de Logros/Logro")]
public class LogroSO : ScriptableObject
{
    [Header("Configuración Básica")]
    public string id;
    public string titulo;
    [TextArea] public string descripcion;

    [Header("Progreso")]
    public int meta; 
    public int progresoActual; 
    
    [Header("Premios")]
    public int monedasRecompensa; 
    public bool reclamado; 

    public bool EstaCompletado => progresoActual >= meta;

    [ContextMenu("Reiniciar Progreso")]
    public void Reiniciar()
    {
        progresoActual = 0;
        reclamado = false;
    }
}