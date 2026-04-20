using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;

public class RegisterAPI : MonoBehaviour
{
    private TextField inputUsuario;
    private TextField inputPassword;
    private Button botonRegister;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        inputUsuario = root.Q<TextField>("usuario");
        inputPassword = root.Q<TextField>("password");
        botonRegister = root.Q<Button>("register");

        botonRegister.clicked += RegistrarDesdeUI;
    }

    void RegistrarDesdeUI()
    {
        string usuario = inputUsuario.value;
        string password = inputPassword.value;

        Debug.Log("Registrando usuario: " + usuario);

        StartCoroutine(RegisterRequest(usuario, password));
    }

    IEnumerator RegisterRequest(string usuario, string password)
    {
        string url = "http://192.168.68.115:3000/register";

        string json = JsonUtility.ToJson(new LoginData(usuario, password));

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Usuario registrado");
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
