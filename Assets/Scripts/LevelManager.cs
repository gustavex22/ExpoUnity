using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestor de niveles sencillo.
/// Encapsula la carga y reinicio de escenas del laberinto.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Índice de la escena del menú principal en Build Settings.")]
    public int mainMenuSceneIndex = 0;

    void Start()
    {
        // Opcional: registrar nivel actual en PlayerPrefs para estadísticas
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Maze_LastLevel", currentIndex);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Reinicia la escena activa.
    /// </summary>
    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Carga la escena del menú principal.
    /// </summary>
    public void LoadMainMenu()
    {
        if (mainMenuSceneIndex >= 0 && mainMenuSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(mainMenuSceneIndex);
        }
        else
        {
            Debug.LogWarning($"LevelManager: La escena de menú {mainMenuSceneIndex} no existe en Build Settings.");
        }
    }

    /// <summary>
    /// Carga el siguiente nivel disponible.
    /// </summary>
    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("LevelManager: No hay más niveles. Volviendo al menú.");
            LoadMainMenu();
        }
    }
}
