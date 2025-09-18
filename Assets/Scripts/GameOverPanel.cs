using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// Этот метод должен быть вызван кнопкой "Начать сначала".
    /// </summary>
    public void OnRestartButtonClick()
    {
        // Вызываем метод перезапуска из GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }

    /// <summary>
    /// Этот метод должен быть вызван кнопкой "Выход в меню".
    /// </summary>
    public void OnExitToMenuButtonClick()
    {
        // Загружаем сцену главного меню.
        // Убедитесь, что у вас есть сцена с именем "MainMenu" и она добавлена в Build Settings.
        SceneManager.LoadScene("MainMenu");
    }
}