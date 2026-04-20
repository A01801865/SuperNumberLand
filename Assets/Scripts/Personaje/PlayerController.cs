using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 8f;

    [Header("Vidas")]
    public int vidas = 3;
    public float limiteCaida = -6f;

    [Header("Respawn")]
    public Transform puntoRespawn;

    [Header("Límites del mapa")]
    public float limiteIzq = -10f;
    public float limiteDer = 10f;

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

        movimiento = Input.GetAxis("Horizontal");
        animator.SetFloat("velocidad", Mathf.Abs(movimiento));

        // Voltear personaje
        if (movimiento != 0)
            sr.flipX = movimiento < 0;

        // SALTO
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            animator.SetTrigger("salto");
        }

        // ATAQUE (SHIFT)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("atacar");
            DetectarGolpe();
        }

        // LÍMITES
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteIzq, limiteDer);
        transform.position = pos;

        // CAÍDA AL VACÍO
        if (transform.position.y < limiteCaida)
        {
            Debug.Log("CAYO AL VACIO");
            PerderVida();
        }
    }

    void FixedUpdate()
    {
        if (muerto) return;

        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);
    }

    // DETECTAR GOLPE
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

    // DETECTAR SUELO
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            enSuelo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            enSuelo = false;
        }
    }

    // PERDER VIDA
    public void PerderVida()
    {
        if (muerto) return;

        vidas--;

        Debug.Log("VIDA PERDIDA → " + vidas);

        if (uiVidas != null)
            uiVidas.ActualizarVidas(vidas);

        if (vidas <= 0)
        {
            Debug.Log("GAME OVER");
            muerto = true;

            if (uiVidas != null)
                uiVidas.MostrarPantallaPerder();
        }
        else
        {
            Respawn();
        }
    }

    // RESPAWN
    void Respawn()
    {
        transform.position = puntoRespawn.position;
        rb.linearVelocity = Vector2.zero;
    }

    // DEBUG VISUAL
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoGolpe);
    }
}