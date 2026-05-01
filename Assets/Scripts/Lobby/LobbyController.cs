using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LobbyController : MonoBehaviour
{
    [Header("Sprites de fondos")]
    public Sprite fondoDefault;
    public Sprite fondoNoche;
    public Sprite fondoArboles;
    public Sprite fondoNubes;

    [Header("Sprites de personajes")]
    public Sprite personaje0Sprite;
    public Sprite personaje1Sprite;
    public Sprite personaje2Sprite;
    public Sprite personaje3Sprite;

    private SpriteRenderer fondoRenderer;
    private VisualElement barraVerde;

    void Start()
    {
        GameObject backObj = GameObject.Find("back");
        if (backObj != null)
            fondoRenderer = backObj.GetComponent<SpriteRenderer>();

        var root       = GetComponent<UIDocument>().rootVisualElement;
        var numMonedas = root.Q<Label>("NumMonedas");
        var apodo      = root.Q<TextField>("ApodoJugador");
        barraVerde     = root.Q<VisualElement>("BarraVerde");

        string usuario = PlayerPrefs.GetString("usuario", "");
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);

        if (apodo != null && !string.IsNullOrEmpty(usuario))
            apodo.value = usuario;

        if (id_usuario == 0) id_usuario = 6;

        if (numMonedas != null)
            StartCoroutine(CargarMonedas(id_usuario, numMonedas));

        StartCoroutine(CargarSeleccion(id_usuario));
        StartCoroutine(CargarLogros(id_usuario));
    }

    IEnumerator CargarMonedas(int id_usuario, Label numMonedas)
    {
        string url = $"https://supernumberland-backend.onrender.com/monedas/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            MonedasResponse res = JsonUtility.FromJson<MonedasResponse>(req.downloadHandler.text);
            if (res.success && numMonedas != null)
                numMonedas.text = res.monedas.ToString();
        }
    }

    IEnumerator CargarSeleccion(int id_usuario)
    {
        string url = $"https://supernumberland-backend.onrender.com/seleccion/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            SeleccionResponse res = JsonUtility.FromJson<SeleccionResponse>(req.downloadHandler.text);
            if (res.success)
            {
                PlayerPrefs.SetInt("personaje_seleccionado", res.personaje_seleccionado);
                PlayerPrefs.Save();

                if (fondoRenderer != null)
                {
                    fondoRenderer.sprite = ObtenerSpriteFondo(res.fondo_seleccionado);
                    AjustarEscala();
                }

                var root    = GetComponent<UIDocument>().rootVisualElement;
                var imgPerf = root.Q<VisualElement>("ImgPerf");
                if (imgPerf != null)
                {
                    Sprite spritePersonaje = ObtenerSpritePersonaje(res.personaje_seleccionado);
                    if (spritePersonaje != null)
                    {
                        imgPerf.style.backgroundImage               = new StyleBackground(spritePersonaje);
                        imgPerf.style.backgroundSize                = new BackgroundSize(BackgroundSizeType.Contain);
                        imgPerf.style.unityBackgroundImageTintColor = Color.white;
                    }
                }

                ActualizarPersonajeEnLobby(res.personaje_seleccionado);
            }
        }
    }

    IEnumerator CargarLogros(int id_usuario)
    {
        string url = $"https://supernumberland-backend.onrender.com/logros/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success) yield break;

        LogrosResponse res = JsonUtility.FromJson<LogrosResponse>(req.downloadHandler.text);
        if (!res.success) yield break;

        int totalLogros = res.logros.Count;
        int desbloqueados = 0;
        foreach (var logro in res.logros)
            if (logro.desbloqueado == 1) desbloqueados++;

        //Actualizar barra verde
        if (barraVerde != null && totalLogros > 0)
        {
            float porcentaje = (float)desbloqueados / totalLogros * 100f;
            barraVerde.style.width = Length.Percent(porcentaje);
        }

        Debug.Log($"Logros: {desbloqueados}/{totalLogros}");
    }

    void ActualizarPersonajeEnLobby(int id_personaje)
    {
        int index = ObtenerIndexPersonaje(id_personaje);

        if (GameManager.instancia != null)
            GameManager.instancia.personajeSeleccionado = index;

        MostrarPersonajeLobby lobby = FindFirstObjectByType<MostrarPersonajeLobby>();
        if (lobby != null)
            lobby.MostrarPersonaje(index);
    }

    int ObtenerIndexPersonaje(int id_item)
    {
        switch (id_item)
        {
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            default: return 0;
        }
    }

    Sprite ObtenerSpritePersonaje(int id_item)
    {
        switch (id_item)
        {
            case 1: return personaje1Sprite;
            case 2: return personaje2Sprite;
            case 3: return personaje3Sprite;
            default: return personaje0Sprite;
        }
    }

    void AjustarEscala()
    {
        if (fondoRenderer == null || fondoRenderer.sprite == null) return;

        Camera cam = Camera.main;
        float alturaWorld  = cam.orthographicSize * 2f;
        float anchoWorld   = alturaWorld * cam.aspect;
        float alturaSprite = fondoRenderer.sprite.bounds.size.y;
        float anchoSprite  = fondoRenderer.sprite.bounds.size.x;

        float scaleX = anchoWorld  / anchoSprite;
        float scaleY = alturaWorld / alturaSprite;

        fondoRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }

    Sprite ObtenerSpriteFondo(int id_item)
    {
        switch (id_item)
        {
            case 4: return fondoNoche;
            case 5: return fondoArboles;
            case 6: return fondoNubes;
            case 7: return fondoDefault;
            default: return fondoDefault;
        }
    }
}