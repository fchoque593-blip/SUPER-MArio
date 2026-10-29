
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoBasicoMario : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;

    private Rigidbody2D rb;
    private bool enSuelo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            horizontal * velocidad,
            rb.linearVelocity.y
        );

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            enSuelo = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ComprobarSuelo(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ComprobarSuelo(collision);
    }

    private void ComprobarSuelo(Collision2D collision)
    {
        foreach (ContactPoint2D contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.5f)
            {
                enSuelo = true;
                return;
            }
        }
    }
}
