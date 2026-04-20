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
        if (muerto) return; // 🔥 bloquea control al morir

        movimiento = Input.GetAxis("Horizontal");
        animator.SetFloat("velocidad", Mathf.Abs(movimiento));

        // Voltear
        if (movimiento != 0)
            sr.flipX = movimiento < 0;

        // SALTO
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            animator.SetTrigger("salto");
        }

        // ATAQUE
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("atacar");
        }

        // LÍMITES
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteIzq, limiteDer);
        transform.position = pos;

        // CAÍDA
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

    // DETECTAR SUELO (con tu trigger)
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
    void PerderVida()
    {
        vidas--;

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
}