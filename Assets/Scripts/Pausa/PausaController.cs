using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PausaController : MonoBehaviour
{
    public bool estaPausado = false;

    private VisualElement pausa;
    private VisualElement pantallaPerder;
    private VisualElement pantallaGanar;
    private Button botonContinuar;
    private InputAction accionPausar;

    void Awake()
    {
        accionPausar = new InputAction(
            name: "Pausar",
            type: InputActionType.Button,
            binding: "<Keyboard>/escape"
        );

        accionPausar.performed += OnPause;
    }

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pausa = root.Q<VisualElement>("Pausa");
        botonContinuar = root.Q<Button>("BotonContinuar");

        // ← Obtener referencias a pantallas de ganar y perder
        pantallaPerder = root.Q<VisualElement>("Perder");
        pantallaGanar = root.Q<VisualElement>("Ganar");

        pausa.style.display = DisplayStyle.None;

        botonContinuar.clicked += ContinuarJuego;

        accionPausar.Enable();
    }

    void OnDisable()
    {
        accionPausar.Disable();
        botonContinuar.clicked -= ContinuarJuego;
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        // ← No pausar si está la pantalla de perder o ganar visible
        bool perderVisible = pantallaPerder != null &&
            pantallaPerder.style.display == DisplayStyle.Flex;
        bool ganarVisible = pantallaGanar != null &&
            pantallaGanar.style.display == DisplayStyle.Flex;

        if (perderVisible || ganarVisible) return;

        Pausar();
    }

    public void Pausar()
    {
        estaPausado = !estaPausado;

        pausa.style.display = estaPausado ? DisplayStyle.Flex : DisplayStyle.None;
        Time.timeScale = estaPausado ? 0f : 1f;
    }

    private void ContinuarJuego()
    {
        if (!estaPausado) return;

        pausa.style.display = DisplayStyle.None;
        estaPausado = false;
        Time.timeScale = 1f;
    }
}