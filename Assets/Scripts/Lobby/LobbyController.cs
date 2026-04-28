using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class LobbyController : MonoBehaviour
{
    public Sprite fondoDefault;
    public Sprite fondoNoche;
    public Sprite fondoArboles;
    public Sprite fondoNubes;

    private SpriteRenderer fondoRenderer;

    void Start()
    {
        GameObject backObj = GameObject.Find("back");
        if (backObj != null)
            fondoRenderer = backObj.GetComponent<SpriteRenderer>();

        var root       = GetComponent<UIDocument>().rootVisualElement;
        var numMonedas = root.Q<Label>("NumMonedas");
        var apodo      = root.Q<TextField>("ApodoJugador");

        string usuario = PlayerPrefs.GetString("usuario", "");
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);

        if (apodo != null && !string.IsNullOrEmpty(usuario))
            apodo.value = usuario;

        if (id_usuario == 0) id_usuario = 6;

        if (numMonedas != null)
            StartCoroutine(CargarMonedas(id_usuario, numMonedas));

        StartCoroutine(CargarFondo(id_usuario));
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

    IEnumerator CargarFondo(int id_usuario)
    {
        string url = $"https://supernumberland-backend.onrender.com/seleccion/{id_usuario}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            SeleccionResponse res = JsonUtility.FromJson<SeleccionResponse>(req.downloadHandler.text);
            if (res.success && fondoRenderer != null)
            {
                fondoRenderer.sprite = ObtenerSpriteFondo(res.fondo_seleccionado);
                AjustarEscala();
            }
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