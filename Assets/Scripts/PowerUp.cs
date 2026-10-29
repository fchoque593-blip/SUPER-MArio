
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Tipo de objeto")]
    public bool moneda;
    public bool vidaExtra;
    public bool estrella;
    public bool champinon;

    [Header("Configuración")]
    public int valorMoneda = 1;

    private bool recogido;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogido)
            return;

        // Solo recoger el objeto si entra Mario.
        if (!other.CompareTag("Player"))
            return;

        Collect(other.gameObject);
    }

    public void Collect(GameObject player)
    {
        if (recogido || player == null)
            return;

        recogido = true;

        // Buscar el GameManager sin asumir que existe.
        GameManager manager = GameManager.Instance;

        if (moneda)
        {
            if (manager != null)
            {
                for (int i = 0; i < valorMoneda; i++)
                    manager.AddCoin();
            }

            Destroy(gameObject);
            return;
        }

        if (vidaExtra)
        {
            if (manager != null)
                manager.AddLife();

            Destroy(gameObject);
            return;
        }

        if (estrella)
        {
            // El efecto de estrella depende de las mecánicas
            // que tenga implementadas tu proyecto.
            Destroy(gameObject);
            return;
        }

        if (champinon)
        {
            // El crecimiento de Mario depende de los scripts
            // originales del jugador.
            Destroy(gameObject);
            return;
        }

        // Si el prefab no tiene tipo asignado, no provocar
        // una excepción ni destruirlo accidentalmente.
        recogido = false;
    }
}
