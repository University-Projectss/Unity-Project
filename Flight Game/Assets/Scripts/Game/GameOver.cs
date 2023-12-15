using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _gameOverCrashText;

    [SerializeField]
    private TextMeshProUGUI _gameOverTimeOutText;

    [SerializeField]
    private GameObject _plane;

    public void ShowGameOverTime()
    {
        PauseGame();
        _gameOverTimeOutText.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void ShowGameOverCrash()
    {
        PauseGame();
        _gameOverCrashText.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void RestartGameButton()
    {
        SceneManager.LoadScene("Main Scene");
        gameObject.SetActive(false);
        ResumeGame();
    }

    public void QuitGameButton() => Application.Quit();

    public void ResumeGame()
    {
        _plane.GetComponent<PlayerInput>().enabled = true;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        _plane.GetComponent<PlayerInput>().enabled = false;
        Time.timeScale = 0;
    }
}
