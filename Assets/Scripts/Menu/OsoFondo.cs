using UnityEngine;

public class OsoFondo : MonoBehaviour
{
    public float velocidad = 2f;
    private bool derecha = true;

    void Update()
    {
        if (derecha)
        {
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            if (transform.position.x > 8)
            {
                derecha = false;
                Voltear();
            }
        }
        else
        {
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);
            if (transform.position.x < -8)
            {
                derecha = true;
                Voltear();
            }
        }
    }

    void Voltear()
    {
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}