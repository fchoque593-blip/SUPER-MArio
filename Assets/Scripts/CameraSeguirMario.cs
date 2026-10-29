
using UnityEngine;

public class CameraSeguirMario : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;

    [Header("Configuración de cámara")]
    public float suavidad = 5f;
    public float desplazamientoX = 2f;
    public float desplazamientoY = 2f;

    private float posicionInicialY;

    private void Start()
    {
        posicionInicialY = transform.position.y;
        BuscarMario();
    }

    private void LateUpdate()
    {
        if (objetivo == null)
        {
            BuscarMario();
            if (objetivo == null)
                return;
        }

        Vector3 destino = new Vector3(
            objetivo.position.x + desplazamientoX,
            posicionInicialY + desplazamientoY,
            -10f
        );

        transform.position = Vector3.Lerp(
            transform.position,
            destino,
            suavidad * Time.deltaTime
        );
    }

    private void BuscarMario()
    {
        GameObject mario = GameObject.Find("Mario(Clone)");

        if (mario == null)
            mario = GameObject.Find("Mario");

        if (mario != null)
            objetivo = mario.transform;
    }
}
