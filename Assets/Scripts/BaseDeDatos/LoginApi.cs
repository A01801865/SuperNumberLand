using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement; //para cambiar escena

public class LoginAPI : MonoBehaviour
{
    private TextField inputUsuario;
    private TextField inputPassword;
    private Button botonLogin;

    void Start()
    {
        // Obtener el UI Document
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Buscar elementos por name (deben coincidir con UI Builder)
        inputUsuario = root.Q<TextField>("usuario");
        inputPassword = root.Q<TextField>("password");
        botonLogin = root.Q<Button>("login");

        // Asignar evento al botón
        botonLogin.clicked += LoginDesdeUI;
    }

    void LoginDesdeUI()
    {
        string usuario = inputUsuario.value;
        string password = inputPassword.value;

        Debug.Log("Intentando login con: " + usuario);

        StartCoroutine(LoginRequest(usuario, password));
    }

    IEnumerator LoginRequest(string usuario, string password)
    {
        string url = "http://192.168.68.115:3000/login";

        string json = JsonUtility.ToJson(new LoginData(usuario, password));

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string respuesta = request.downloadHandler.text;
            Debug.Log("Respuesta: " + respuesta);

            // Validar login
            if (respuesta.Contains("\"success\":true"))
            {
                Debug.Log("Login correcto");

                // Cambiar de escena
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                Debug.Log("Login incorrecto");
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}

//  Clase para enviar JSON
[System.Serializable]
public class LoginData
{
    public string usuario;
    public string contrasena;

    public LoginData(string u, string c)
    {
        usuario = u;
        contrasena = c;
    }
}