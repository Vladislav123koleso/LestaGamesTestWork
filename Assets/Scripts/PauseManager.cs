using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject pauseMenuPanel; // Ссылка на панель паузы

    void Update()
    {
        // Проверяем нажатие клавиши ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Ставит игру на паузу.
    /// </summary>
    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Time.timeScale = 0f; // Останавливаем время в игре
        IsPaused = true;
    }

    /// <summary>
    /// Снимает игру с паузы. Этот метод будет вызываться кнопкой "Продолжить".
    /// </summary>
    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f; // Возобновляем течение времени
        IsPaused = false;
    }

    /// <summary>
    /// Выходит в главное меню. Этот метод будет вызываться кнопкой "Выйти в меню".
    /// </summary>
    public void GoToMainMenu()
    {
        // Важно сбросить Time.timeScale перед загрузкой другой сцены
        Time.timeScale = 1f;
        IsPaused = false;
        
        // Уничтожаем GameManager, чтобы он не дублировался при возвращении в игру
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        // Убедитесь, что у вас есть сцена с именем "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
}