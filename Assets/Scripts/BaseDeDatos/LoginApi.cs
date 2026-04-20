using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LoginAPI : MonoBehaviour
{
    private TextField inputUsuario;
    private TextField inputPassword;
    private Button botonLogin;
    private Label mensaje;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        inputUsuario = root.Q<TextField>("usuario");
        inputPassword = root.Q<TextField>("password");
        botonLogin = root.Q<Button>("login");
        mensaje = root.Q<Label>("mensaje");

        botonLogin.clicked += LoginDesdeUI;
    }

    void LoginDesdeUI()
    {
        string usuario = inputUsuario.value.Trim();
        string password = inputPassword.value.Trim();

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
        {
            Debug.Log("⚠️ Campos vacíos");
            if (mensaje != null) mensaje.text = "Completa todos los campos";
            return;
        }

        Debug.Log("🔐 Intentando login con: " + usuario);

        StartCoroutine(LoginRequest(usuario, password));
    }

    IEnumerator LoginRequest(string usuario, string password)
    {
        string url = "https://supernumberland-backend.onrender.com/login";

        LoginData data = new LoginData(usuario, password);
        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string respuesta = request.downloadHandler.text;

            // 🔥 DEBUG IMPORTANTE
            Debug.Log("📩 RESPUESTA COMPLETA: " + respuesta);

            LoginResponse res = JsonUtility.FromJson<LoginResponse>(respuesta);

            if (res.success)
            {
                Debug.Log("✅ LOGIN EXITOSO");

                // 🔥 DEBUG DE DATOS
                Debug.Log("👤 Usuario: " + res.user.usuario);
                Debug.Log("🎂 Edad recibida: " + res.user.edad);

                // 🔥 GUARDAR DATOS
                PlayerPrefs.SetInt("user_id", res.user.id);
                PlayerPrefs.SetString("usuario", res.user.usuario);
                PlayerPrefs.SetString("nombre", res.user.nombre);
                PlayerPrefs.SetInt("edad", res.user.edad);

                PlayerPrefs.Save();

                // 🔥 CONFIRMAR QUE SE GUARDÓ
                Debug.Log("💾 Edad guardada en PlayerPrefs: " + PlayerPrefs.GetInt("edad"));

                // 🚀 CAMBIAR ESCENA
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                Debug.Log("❌ " + res.message);
                if (mensaje != null) mensaje.text = res.message;
            }
        }
        else
        {
            Debug.LogError("❌ ERROR HTTP: " + request.error);
        }
    }
}

#region CLASES JSON

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

[System.Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
    public User user;
}

[System.Serializable]
public class User
{
    public int id;
    public string usuario;
    public string nombre;
    public int edad;
    public string genero;
}

#endregion