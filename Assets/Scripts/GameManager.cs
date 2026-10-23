using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestor central del juego (Singleton).
/// Lleva el conteo de monedas, temporizador, récords guardados y condición de victoria.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencias de UI")]
    [Tooltip("Texto que muestra las monedas recolectadas.")]
    public Text coinText;

    [Tooltip("Texto que muestra el temporizador del nivel.")]
    public Text timerText;

    [Tooltip("Panel que se activa al ganar la partida.")]
    public GameObject victoryPanel;

    [Tooltip("Texto del mensaje de victoria dentro del panel.")]
    public Text victoryText;

    [Tooltip("Botón para reiniciar el nivel (opcional, se crea por código si falta).")]
    public Button restartButton;

    [Tooltip("Botón para volver al menú principal (opcional, se crea por código si falta).")]
    public Button menuButton;

    [Header("Estado del juego")]
    [Tooltip("Cantidad total de monedas en la escena (se calcula automáticamente en Start).")]
    public int totalCoins;

    private int collectedCoins;
    private float levelTimer;
    private bool gameActive;
    private bool hasWon;

    // Claves de PlayerPrefs
    private const string PrefBestCoins = "Maze_BestCoins";
    private const string PrefBestTime = "Maze_BestTime";

    void Awake()
    {
        // Implementación de Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Contar automáticamente todas las monedas con tag "Coin" al iniciar
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        totalCoins = coins.Length;

        collectedCoins = 0;
        levelTimer = 0f;
        gameActive = true;
        hasWon = false;

        // Inicializar UI
        UpdateCoinUI();
        UpdateTimerUI();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        // Configurar botones si existen
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartLevel);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(GoToMainMenu);
        }
    }

    void Update()
    {
        if (!gameActive || hasWon) return;

        levelTimer += Time.deltaTime;
        UpdateTimerUI();
    }

    /// <summary>
    /// Llama este método desde Coin cuando el jugador recolecta una moneda.
    /// </summary>
    public void CollectCoin()
    {
        if (!gameActive || hasWon) return;

        collectedCoins++;
        UpdateCoinUI();

        // Guardar récord de monedas si se supera el anterior
        int bestCoins = PlayerPrefs.GetInt(PrefBestCoins, 0);
        if (collectedCoins > bestCoins)
        {
            PlayerPrefs.SetInt(PrefBestCoins, collectedCoins);
            PlayerPrefs.Save();
        }

        // Condición de victoria: ya no quedan monedas por recolectar
        if (collectedCoins >= totalCoins && totalCoins > 0)
        {
            TriggerVictory();
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = $"Monedas: {collectedCoins} / {totalCoins}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(levelTimer / 60f);
            int seconds = Mathf.FloorToInt(levelTimer % 60f);
            timerText.text = $"Tiempo: {minutes:00}:{seconds:00}";
        }
    }

    private void TriggerVictory()
    {
        hasWon = true;
        gameActive = false;

        // Guardar mejor tiempo si es el primero o es menor
        float bestTime = PlayerPrefs.GetFloat(PrefBestTime, 9999f);
        if (levelTimer < bestTime)
        {
            PlayerPrefs.SetFloat(PrefBestTime, levelTimer);
            PlayerPrefs.Save();
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (victoryText != null)
        {
            int minutes = Mathf.FloorToInt(levelTimer / 60f);
            int seconds = Mathf.FloorToInt(levelTimer % 60f);
            victoryText.text = $"¡VICTORIA!\nMonedas: {collectedCoins}/{totalCoins}\nTiempo: {minutes:00}:{seconds:00}";
        }

        Debug.Log("¡VICTORIA! Todas las monedas han sido recolectadas.");
    }

    /// <summary>
    /// Reinicia la escena activa (nivel actual).
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Carga la escena del menú principal (índice 0) si existe.
    /// </summary>
    public void GoToMainMenu()
    {
        if (SceneManager.sceneCountInBuildSettings > 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.LogWarning("No hay escena de menú configurada en Build Settings.");
        }
    }

    /// <summary>
    /// Devuelve el tiempo transcurrido del nivel actual.
    /// </summary>
    public float GetLevelTime() => levelTimer;

    /// <summary>
    /// Devuelve si el juego ha terminado (victoria).
    /// </summary>
    public bool HasWon() => hasWon;
}
