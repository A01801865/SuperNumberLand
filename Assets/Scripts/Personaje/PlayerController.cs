using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;

    public int vidas = 3;
    public float limiteCaida = -6f;

    public Transform puntoRespawn;
    public float limiteIzq = -10f;
    public float limiteDer = 10f;

    public UIVidasToolkit uiVidas;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private float movimiento;
    private bool enSuelo;

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
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);
    }

    // 🔥 DETECCIÓN DE SUELO CON TRIGGER
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

    void PerderVida()
    {
        vidas--;

        if (uiVidas != null)
            uiVidas.ActualizarVidas(vidas);

        if (vidas <= 0)
        {
            Debug.Log("GAME OVER");
        }
        else
        {
            transform.position = puntoRespawn.position;
            rb.linearVelocity = Vector2.zero;
        }
    }
}