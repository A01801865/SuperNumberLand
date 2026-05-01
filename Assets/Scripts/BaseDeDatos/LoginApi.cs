using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LoginAPI : MonoBehaviour
{
    // Referencias a los elementos de la UI
    private TextField inputUsuario;
    private TextField inputPassword;
    private Button botonLogin;
    private Label mensaje;

    void OnEnable()
    {
        // Obtener los elementos de la UI desde el documento
        var root = GetComponent<UIDocument>().rootVisualElement;

        inputUsuario  = root.Q<TextField>("usuario");
        inputPassword = root.Q<TextField>("password");
        botonLogin    = root.Q<Button>("login");
        mensaje       = root.Q<Label>("mensaje");

        // Suscribir el botón al método de login
        botonLogin.clicked += LoginDesdeUI;
    }

    void LoginDesdeUI()
    {
        // Leer y limpiar los valores ingresados por el jugador
        string usuario  = inputUsuario.value.Trim();
        string password = inputPassword.value.Trim();

        // Validar que los campos no estén vacíos antes de enviar
        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
        {
            if (mensaje != null) mensaje.text = "Completa todos los campos";
            return;
        }

        StartCoroutine(LoginRequest(usuario, password));
    }

    // Envía las credenciales al servidor y procesa la respuesta
    IEnumerator LoginRequest(string usuario, string password)
    {
        string url = "https://supernumberland-backend.onrender.com/login";

        // Serializar los datos de login a JSON
        LoginData data = new LoginData(usuario, password);
        string json = JsonUtility.ToJson(data);

        // Construir la petición POST con el JSON en el cuerpo
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string respuesta = request.downloadHandler.text;
            LoginResponse res = JsonUtility.FromJson<LoginResponse>(respuesta);

            if (res.success)
            {
                // Guardar los datos del jugador localmente para usarlos en otras escenas
                PlayerPrefs.SetInt("user_id",    res.user.id);
                PlayerPrefs.SetString("usuario", res.user.usuario);
                PlayerPrefs.SetString("nombre",  res.user.nombre);
                PlayerPrefs.SetInt("edad",       res.user.edad);
                PlayerPrefs.Save();

                // Ir al Lobby si el login fue exitoso
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                // Mostrar el mensaje de error devuelto por el servidor
                if (mensaje != null) mensaje.text = res.message;
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}

#region CLASES JSON

// Datos que se envían al servidor para autenticar al jugador
[System.Serializable]
public class LoginData
{
    public string usuario;
    public string contrasena;

    public LoginData(string u, string c)
    {
        usuario   = u;
        contrasena = c;
    }
}

// Respuesta del servidor al intentar iniciar sesión
[System.Serializable]
public class LoginResponse
{
    public bool   success;
    public string message;
    public User   user;
}

// Datos del jugador devueltos por el servidor tras un login exitoso
[System.Serializable]
public class User
{
    public int    id;
    public string usuario;
    public string nombre;
    public int    edad;
    public string genero;
}

#endregion