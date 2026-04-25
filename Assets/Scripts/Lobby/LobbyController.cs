using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class LobbyController : MonoBehaviour
{
    public Sprite fondoDefault;        // back
    public Sprite fondoNoche;          // parallax-mountain-bg
    public Sprite fondoArboles;        // frame0000
    public Sprite fondoNubes;          // demo01_PixelSky

    public SpriteRenderer fondoRenderer; // arrastrar el GameObject "back"

    void OnEnable()
    {
        var root       = GetComponent<UIDocument>().rootVisualElement;
        var apodo      = root.Q<TextField>("ApodoJugador");
        var numMonedas = root.Q<Label>("NumMonedas");

        string usuario = PlayerPrefs.GetString("usuario", "");
        int id_usuario = PlayerPrefs.GetInt("user_id", 0);

        if (apodo != null && !string.IsNullOrEmpty(usuario))
            apodo.value = usuario;

        if (id_usuario == 0) id_usuario = 6;

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
            }
        }
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