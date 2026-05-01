using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RegisterAPI : MonoBehaviour
{
    // Referencias a los campos de texto del formulario de registro
    private TextField inputUsuario;
    private TextField inputPassword;
    private TextField inputNombre;
    private TextField inputAlcaldia;
    private TextField inputActividad;
    private TextField inputEdad;

    // Dropdowns para género y si el jugador realiza alguna actividad
    private DropdownField dropdownGenero;
    private DropdownField dropdownActividad;

    private Button botonRegister;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Obtener campos de texto desde la UI
        inputUsuario   = root.Q<TextField>("usuario");
        inputPassword  = root.Q<TextField>("password");
        inputNombre    = root.Q<TextField>("nombre");
        inputAlcaldia  = root.Q<TextField>("alcaldia");
        inputActividad = root.Q<TextField>("actividad");
        inputEdad      = root.Q<TextField>("edad");

        // Obtener dropdowns desde la UI
        dropdownGenero    = root.Q<DropdownField>("genero");
        dropdownActividad = root.Q<DropdownField>("realizaActividad");

        botonRegister = root.Q<Button>("register");

        // Verificar que los elementos críticos existen en la UI
        if (inputActividad == null)
            Debug.LogError("inputActividad NO encontrado en el UI");

        if (dropdownActividad == null)
            Debug.LogError("dropdownActividad NO encontrado");

        if (botonRegister == null)
            Debug.LogError("botón register NO encontrado");

        // Suscribir el botón al método de registro
        botonRegister.clicked += RegistrarDesdeUI;

        // Mostrar u ocultar el campo de actividad según la selección del dropdown
        if (dropdownActividad != null)
        {
            dropdownActividad.RegisterValueChangedCallback(evt =>
            {
                CambiarVisibilidadActividad(evt.newValue);
            });
        }
    }

    // Muestra el campo de actividad si el jugador seleccionó "Sí", lo oculta si seleccionó "No"
    void CambiarVisibilidadActividad(string valor)
    {
        if (inputActividad == null)
        {
            Debug.LogError("inputActividad es NULL en CambiarVisibilidadActividad");
            return;
        }

        if (valor.ToLower() == "no")
        {
            inputActividad.value = "";
            inputActividad.style.display = DisplayStyle.None;
        }
        else
        {
            inputActividad.style.display = DisplayStyle.Flex;
        }
    }

    void RegistrarDesdeUI()
    {
        // Verificar que los campos obligatorios están disponibles antes de continuar
        if (inputUsuario == null || inputPassword == null || inputEdad == null)
        {
            Debug.LogError("Faltan referencias de UI");
            return;
        }

        // Leer los valores del formulario (usar cadena vacía si el campo es nulo)
        string usuario   = inputUsuario.value;
        string password  = inputPassword.value;
        string nombre    = inputNombre?.value    ?? "";
        string alcaldia  = inputAlcaldia?.value  ?? "";
        string actividad = inputActividad?.value ?? "";
        string genero    = dropdownGenero?.value ?? "";

        // Convertir edad a entero; quedará en 0 si el valor no es numérico
        int edad = 0;
        int.TryParse(inputEdad.value, out edad);

        // Si el jugador indicó que no realiza actividad, enviar el campo vacío
        if (dropdownActividad != null && dropdownActividad.value.ToLower() == "no")
            actividad = "";

        Debug.Log("Registrando completo");

        StartCoroutine(RegisterRequest(
            usuario, password, nombre, alcaldia, actividad, edad, genero
        ));
    }

    // Envía los datos del nuevo jugador al servidor y redirige al Lobby si el registro es exitoso
    IEnumerator RegisterRequest(string usuario, string password, string nombre,
        string alcaldia, string actividad, int edad, string genero)
    {
        string url = "https://supernumberland-backend.onrender.com/register";

        // Serializar los datos del formulario a JSON
        RegisterData data = new RegisterData(usuario, password, nombre, alcaldia, actividad, edad, genero);
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
            Debug.Log("REGISTRO COMPLETO");

            // Guardar datos básicos del jugador localmente para usarlos en otras escenas
            PlayerPrefs.SetString("usuario", usuario);
            PlayerPrefs.SetInt("edad", edad);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Lobby");
        }
    }
}

// Estructura que agrupa los datos del formulario para enviarlos como JSON al servidor
[System.Serializable]
public class RegisterData
{
    public string usuario;
    public string contrasena;
    public string nombre_completo;
    public string alcaldia;
    public string actividad;
    public int    edad;
    public string genero;

    public RegisterData(string u, string c, string n, string a, string act, int e, string g)
    {
        usuario         = u;
        contrasena      = c;
        nombre_completo = n;
        alcaldia        = a;
        actividad       = act;
        edad            = e;
        genero          = g;
    }
}