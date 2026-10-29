
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Botones de la escena")]
    public Button botonReintentar;
    public Button botonSalir;

    private void Start()
    {
        if (botonReintentar != null)
        {
            botonReintentar.onClick.RemoveListener(VolverAJugar);
            botonReintentar.onClick.AddListener(VolverAJugar);
        }

        if (botonSalir != null)
        {
            botonSalir.onClick.RemoveListener(SalirDelJuego);
            botonSalir.onClick.AddListener(SalirDelJuego);
        }
    }

    public void VolverAJugar()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }
        else
        {
            SceneManager.LoadScene("1-1");
        }
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
