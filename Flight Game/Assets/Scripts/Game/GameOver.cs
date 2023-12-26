using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private HighscoresDisplay _highscoreDisplay;

    [SerializeField]
    private TextMeshProUGUI _gameOverCrashText;

    [SerializeField]
    private TextMeshProUGUI _gameOverTimeOutText;

    [SerializeField]
    private GameObject _plane;

    public bool Over { get; private set; }

    public void ShowGameOverTime()
    {
        ShowGameOver();
        _gameOverTimeOutText.gameObject.SetActive(true);
    }

    public void ShowGameOverCrash()
    {
        ShowGameOver();
        _gameOverCrashText.gameObject.SetActive(true);
    }

    private void ShowGameOver()
    {
        PauseGame();
        Over = true;
        _highscoreDisplay.WriteScores();
        gameObject.SetActive(true);
    }

    public void RestartGameButton()
    {
        //Change this back to "Main Scene" before PR merge
        //Left like this for easier testing
        SceneManager.LoadScene("Scoring");
        gameObject.SetActive(false);
        Over = false;
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
