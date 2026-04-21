using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 8f;

    [Header("Suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;

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
    private bool muerto = false;

    //  CONTROL DE SALTO
    float tiempoUltimoSalto = 0f;
    public float cooldownSalto = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (uiVidas != null)
            uiVidas.ActualizarVidas(vidas);
    }

    void Update()
    {
        if (muerto) return;

        //  DETECTAR SUELO (piso + cajas)
        int mask = LayerMask.GetMask("Suelo", "Respuestas");
        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, mask);

        movimiento = Input.GetAxis("Horizontal");
        animator.SetFloat("velocidad", Mathf.Abs(movimiento));

        // Voltear personaje
        if (movimiento != 0)
            sr.flipX = movimiento < 0;

        // SALTO
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            && enSuelo
            && Time.time > tiempoUltimoSalto)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            animator.SetTrigger("salto");

            tiempoUltimoSalto = Time.time + cooldownSalto;
        }

        //  ATAQUE
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("atacar");
            DetectarGolpe();
        }

        //  CAÍDA
        if (transform.position.y < limiteCaida)
        {
            PerderVida();
        }
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
            {
                obj.RecibirGolpe();
            }
        }
    }

    public void PerderVida()
    {
        if (muerto) return;

        vidas--;

        Debug.Log("VIDA PERDIDA → " + vidas);

        if (uiVidas != null)
            uiVidas.ActualizarVidas(vidas);

        if (vidas <= 0)
        {
            muerto = true;

            if (uiVidas != null)
                uiVidas.MostrarPantallaPerder();

           
            if (animator != null)
                animator.SetTrigger("morir");

            
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Respawn();
        }
    }

    void Respawn()
    {
        if (puntoRespawn != null)
        {
            transform.position = puntoRespawn.position;
            rb.linearVelocity = Vector2.zero;
        }
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