using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject pauseMenuPanel; // ������ �� ������ �����

    void Update()
    {
        // ��������� ������� ������� ESC
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
    /// ������ ���� �� �����.
    /// </summary>
    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Time.timeScale = 0f; // ������������� ����� � ����
        IsPaused = true;
    }

    /// <summary>
    /// ������� ���� � �����. ���� ����� ����� ���������� ������� "����������".
    /// </summary>
    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f; // ������������ ������� �������
        IsPaused = false;
    }

    /// <summary>
    /// ������� � ������� ����. ���� ����� ����� ���������� ������� "����� � ����".
    /// </summary>
    public void GoToMainMenu()
    {
        // ����� �������� Time.timeScale ����� ��������� ������ �����
        Time.timeScale = 1f;
        IsPaused = false;
        
        // ���������� GameManager, ����� �� �� ������������ ��� ����������� � ����
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        // ���������, ��� � ��� ���� ����� � ������ "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
}