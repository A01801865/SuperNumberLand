using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RegisterAPI : MonoBehaviour
{
    private TextField inputUsuario;
    private TextField inputPassword;
    private TextField inputNombre;
    private TextField inputAlcaldia;
    private TextField inputActividad;
    private TextField inputEdad;

    private DropdownField dropdownGenero;
    private DropdownField dropdownActividad;

    private Button botonRegister;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Inputs
        inputUsuario = root.Q<TextField>("usuario");
        inputPassword = root.Q<TextField>("password");
        inputNombre = root.Q<TextField>("nombre");
        inputAlcaldia = root.Q<TextField>("alcaldia");
        inputActividad = root.Q<TextField>("actividad");
        inputEdad = root.Q<TextField>("edad");

        // Dropdowns
        dropdownGenero = root.Q<DropdownField>("genero");
        dropdownActividad = root.Q<DropdownField>("realizaActividad");

        // Botón
        botonRegister = root.Q<Button>("register");

        // DEBUG para verificar UI
        if (inputActividad == null)
            Debug.LogError("inputActividad NO encontrado en el UI");

        if (dropdownActividad == null)
            Debug.LogError("dropdownActividad NO encontrado");

        if (botonRegister == null)
            Debug.LogError("botón register NO encontrado");

        // Evento botón
        botonRegister.clicked += RegistrarDesdeUI;

        // Evento dropdown (mostrar/ocultar actividad)
        if (dropdownActividad != null)
        {
            dropdownActividad.RegisterValueChangedCallback(evt =>
            {
                CambiarVisibilidadActividad(evt.newValue);
            });
        }
    }

    void CambiarVisibilidadActividad(string valor)
    {
        // PROTECCIÓN PARA QUE NO TRUENE
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
        // Validaciones básicas
        if (inputUsuario == null || inputPassword == null || inputEdad == null)
        {
            Debug.LogError("Faltan referencias de UI");
            return;
        }

        string usuario = inputUsuario.value;
        string password = inputPassword.value;
        string nombre = inputNombre?.value ?? "";
        string alcaldia = inputAlcaldia?.value ?? "";
        string actividad = inputActividad?.value ?? "";
        string genero = dropdownGenero?.value ?? "";

        int edad = 0;
        int.TryParse(inputEdad.value, out edad);

        // lógica actividad opcional
        if (dropdownActividad != null && dropdownActividad.value.ToLower() == "no")
        {
            actividad = "";
        }

        Debug.Log("🔥 Registrando completo");

        StartCoroutine(RegisterRequest(
            usuario, password, nombre, alcaldia, actividad, edad, genero
        ));
    }

    IEnumerator RegisterRequest(string usuario, string password, string nombre,
        string alcaldia, string actividad, int edad, string genero)
    {
        string url = "https://supernumberland-backend.onrender.com/register";

        RegisterData data = new RegisterData(usuario, password, nombre, alcaldia, actividad, edad, genero);
        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ REGISTRO COMPLETO");

                // Guarda datos
                PlayerPrefs.SetString("usuario", usuario);
                PlayerPrefs.SetInt("edad", edad);
                PlayerPrefs.Save();

                // IR AL LOBBY
                SceneManager.LoadScene("Lobby");
            }
    }
}

[System.Serializable]
public class RegisterData
{
    public string usuario;
    public string contrasena;
    public string nombre_completo;
    public string alcaldia;
    public string actividad;
    public int edad;
    public string genero;

    public RegisterData(string u, string c, string n, string a, string act, int e, string g)
    {
        usuario = u;
        contrasena = c;
        nombre_completo = n;
        alcaldia = a;
        actividad = act;
        edad = e;
        genero = g;
    }
}