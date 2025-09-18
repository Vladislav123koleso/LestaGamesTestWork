using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// ���� ����� ������ ���� ������ ������� "������ �������".
    /// </summary>
    public void OnRestartButtonClick()
    {
        // �������� ����� ����������� �� GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }

    /// <summary>
    /// ���� ����� ������ ���� ������ ������� "����� � ����".
    /// </summary>
    public void OnExitToMenuButtonClick()
    {
        // ��������� ����� �������� ����.
        // ���������, ��� � ��� ���� ����� � ������ "MainMenu" � ��� ��������� � Build Settings.
        SceneManager.LoadScene("MainMenu");
    }
}