using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections; // Necesario para el retraso

public class LogrosUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public VisualTreeAsset logroTemplate;
    
    [Header("Datos")]
    public List<LogroSO> listaDeLogros;
    
    private VisualElement contenedor;

    // Usamos Start y una Corrutina para dar tiempo a que la UI cargue
    void Start() 
    {
        StartCoroutine(EsperarYActualizar());
    }

    IEnumerator EsperarYActualizar()
    {
        // Esperamos un frame para que el UIDocument esté listo
        yield return null;

        var uiDocument = GetComponent<UIDocument>();
        
        if (uiDocument == null || uiDocument.rootVisualElement == null)
        {
            Debug.LogWarning("Esperando a que el UIDocument esté listo...");
            yield return new WaitForSeconds(0.1f); // Un ligero retraso extra
            uiDocument = GetComponent<UIDocument>();
        }

        if (uiDocument != null && uiDocument.rootVisualElement != null)
        {
            var root = uiDocument.rootVisualElement;
            contenedor = root.Q<VisualElement>("contenedor-logros"); 

            if (contenedor != null)
            {
                ActualizarInterfaz();
            }
            else
            {
                Debug.LogError("No encontré 'contenedor-logros'. Revisa el nombre en UI Builder.");
            }
        }
    }

    public void ActualizarInterfaz()
    {
        if (contenedor == null) return;
        contenedor.Clear(); 

        foreach (LogroSO logro in listaDeLogros)
        {
            VisualElement fila = logroTemplate.Instantiate();

            // Llenar datos (Asegúrate de que estos nombres coincidan en UI Builder)
            fila.Q<Label>("LabelTitulo").text = logro.titulo;
            fila.Q<Label>("LabelMonedas").text = $"+{logro.monedasRecompensa}";

            ProgressBar barra = fila.Q<ProgressBar>("BarraProgreso");
            if (barra != null)
            {
                barra.highValue = logro.meta;
                barra.value = logro.progresoActual;
                barra.title = $"{logro.progresoActual} / {logro.meta}";
            }

            Button btn = fila.Q<Button>("BotonCobrar");
            if (btn != null)
            {
                if (logro.reclamado) {
                    btn.text = "Canjeado";
                    btn.SetEnabled(false);
                } else if (logro.EstaCompletado) {
                    btn.text = "¡Recoger!";
                    btn.SetEnabled(true);
                    btn.clicked += () => {
                        logro.reclamado = true;
                        ActualizarInterfaz();
                    };
                } else {
                    btn.text = "Bloqueado";
                    btn.SetEnabled(false);
                }
            }

            contenedor.Add(fila);
        }
    }
}