
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;
    public int coins { get; private set; } = 0;

    private bool inicializado;
    private bool reinicioPendiente;
    private bool gameOverActivo;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        if (!inicializado)
        {
            lives = 3;
            coins = 0;
            world = 1;
            stage = 1;
            inicializado = true;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void NewGame()
    {
        CancelInvoke();

        lives = 3;
        coins = 0;
        world = 1;
        stage = 1;

        reinicioPendiente = false;
        gameOverActivo = false;
        inicializado = true;

        SceneManager.LoadScene("1-1");
    }

    public void GameOver()
    {
        if (gameOverActivo)
            return;

        CancelInvoke();
        reinicioPendiente = false;
        gameOverActivo = true;

        SceneManager.LoadScene("GameOver");
    }

    public void LoadLevel(int nuevoWorld, int nuevoStage)
    {
        if (gameOverActivo)
            return;

        world = nuevoWorld;
        stage = nuevoStage;

        reinicioPendiente = false;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        if (gameOverActivo || reinicioPendiente)
            return;

        reinicioPendiente = true;
        Invoke(nameof(ProcesarMuerte), Mathf.Max(0f, delay));
    }

    public void ResetLevel()
    {
        if (gameOverActivo || reinicioPendiente)
            return;

        reinicioPendiente = true;
        ProcesarMuerte();
    }

    private void ProcesarMuerte()
    {
        CancelInvoke(nameof(ProcesarMuerte));
        reinicioPendiente = false;

        if (gameOverActivo)
            return;

        lives--;

        if (lives > 0)
        {
            SceneManager.LoadScene($"{world}-{stage}");
        }
        else
        {
            GameOver();
        }
    }

    public void AddCoin()
    {
        if (gameOverActivo)
            return;

        coins++;

        if (coins >= 100)
        {
            coins -= 100;
            AddLife();
        }
    }

    public void AddLife()
    {
        if (!gameOverActivo)
            lives++;
    }
}
