using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 8f;
    public int maxSaltos = 2;

    [Header("Suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    [Header("Vidas")]
    public int vidas = 3;
    public float limiteCaida = -6f;

    [Header("Respawn")]
    public Transform puntoRespawn;

    [Header("UI")]
    public UIVidasToolkit uiVidas;

    [Header("Ataque")]
    public float rangoGolpe = 1.2f;
    public LayerMask capaRespuestas;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private float movimiento;
    private bool enSuelo;
    private bool estabaEnSuelo;
    private bool muerto = false;

    private int saltosRestantes;

    private static int contadorShift = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        saltosRestantes = maxSaltos;

        // Cargar contador guardado entre sesiones
        contadorShift = PlayerPrefs.GetInt("contador_shift", 0);

        if (puntoRespawn == null)
        {
            GameObject respawn = GameObject.Find("Respawn");
            if (respawn != null)
                puntoRespawn = respawn.transform;
        }

        if (uiVidas == null)
            uiVidas = FindFirstObjectByType<UIVidasToolkit>();

        if (GameManagerProgreso.Instance != null)
            vidas = GameManagerProgreso.Instance.vidasActuales;

        if (uiVidas != null)
            uiVidas.ActualizarVidas(vidas);
    }

    void Update()
    {
        if (muerto) return;

        estabaEnSuelo = enSuelo;
        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);

        if (enSuelo && !estabaEnSuelo)
            saltosRestantes = maxSaltos;

        movimiento = Input.GetAxis("Horizontal");
        animator.SetFloat("velocidad", Mathf.Abs(movimiento));

        if (movimiento != 0)
            sr.flipX = movimiento < 0;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && saltosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            animator.SetTrigger("salto");
            saltosRestantes--;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("atacar");
            DetectarGolpe();

            // Logro: Boton Pro (idBD 8) — presionar Shift 50 veces
            contadorShift++;
            PlayerPrefs.SetInt("contador_shift", contadorShift);
            Debug.Log($"Shift presionado: {contadorShift}/50");
            if (contadorShift >= 50)
                LogrosManager.Instance?.DesbloquearLogro(8);
        }

        if (transform.position.y < limiteCaida)
            PerderVida();
    }

    void FixedUpdate()
    {
        if (muerto) return;
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);
    }

    void DetectarGolpe()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoGolpe, capaRespuestas);

        foreach (Collider2D hit in hits)
        {
            ObjetoRespuesta obj = hit.GetComponent<ObjetoRespuesta>();
            if (obj != null)
                obj.RecibirGolpe();
        }
    }

    public void PerderVida()
    {
        if (muerto) return;

        vidas--;

        if (GameManagerProgreso.Instance != null)
        {
            GameManagerProgreso.Instance.vidasActuales = vidas;
            GameManagerProgreso.Instance.vidasPerdidas++;
        }

        if (uiVidas != null)
            uiVidas.ActualizarVidas(vidas);

        if (vidas <= 0)
        {
            muerto = true;

            if (uiVidas != null)
                uiVidas.MostrarPantallaPerder();

            Destroy(gameObject);
        }
        else
        {
            Respawn();
        }
    }

    public void Detener()
    {
        muerto = true;
        rb.linearVelocity = Vector2.zero;
    }

    void Respawn()
    {
        transform.position = puntoRespawn.position;
        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoGolpe);
    }
}