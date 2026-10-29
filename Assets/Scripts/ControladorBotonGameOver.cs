
using UnityEngine;
using UnityEngine.UI;

public class ControladorBotonGameOver : MonoBehaviour
{
    public Button boton;
    public GameOverUI controlador;

    private void Awake()
    {
        if (boton != null && controlador != null)
            boton.onClick.AddListener(controlador.VolverAJugar);
    }
}
