using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicaManager : MonoBehaviour
{
    public static MusicaManager Instance;
    private AudioSource audioSource;

    [Header("Clips de Audio")]
    public AudioClip musicaMenu;
    public AudioClip musicaLobby;
    public AudioClip musicaSuma;
    public AudioClip musicaResta;
    public AudioClip musicaMulti;
    public AudioClip musicaDivi;

    private string[] escenasMenu = { "Menu", "Reg_InSes", "Registro", "IniciarSesion" };
    private string[] escenasLobby = { "Lobby", "Niveles", "Perfil", "Tienda", "Inventario", "Temas" };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string nombre = scene.name;
        AudioClip clipAdecuado = null;

        if (System.Array.Exists(escenasMenu, e => e == nombre)) clipAdecuado = musicaMenu;
        else if (System.Array.Exists(escenasLobby, e => e == nombre)) clipAdecuado = musicaLobby;
        else if (nombre.StartsWith("Mapa") || nombre == "Nivel1") clipAdecuado = musicaSuma;
        else if (nombre.StartsWith("RNivel")) clipAdecuado = musicaResta;
        else if (nombre.StartsWith("MNivel")) clipAdecuado = musicaMulti;
        else if (nombre.StartsWith("DNivel")) clipAdecuado = musicaDivi;

        GestionarReproduccion(clipAdecuado);
    }

    void GestionarReproduccion(AudioClip nuevoClip)
    {
        if (nuevoClip == null)
        {
            audioSource.Stop();
            return;
        }

        if (audioSource.clip != nuevoClip)
        {
            audioSource.clip = nuevoClip;
            audioSource.Play();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}