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

    // Ставим игру на паузу.
    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Time.timeScale = 0f; 
        IsPaused = true;
    }

    
    // Снимаем игру с паузы.  
    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f; // Возобновляем 
        IsPaused = false;
    }

    // Выходит в главное меню. Этот метод будет вызываться кнопкой "Выйти в меню".
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        
        
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        
        SceneManager.LoadScene("MainMenu");
    }
}