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
    private GameObject _quitButton;

    [SerializeField]
    private GameObject _replayButton;

    [SerializeField]
    private GameObject _plane;

    [SerializeField]
    private GameObject _menuButton;

    [SerializeField]
    private AudioSource _audioSource;

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
        _quitButton.SetActive(true);
        _replayButton.SetActive(true);
        _menuButton.SetActive(true);
    }

    public void RestartGameButton()
    {
        MainMenu.load = false;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        gameObject.SetActive(false);
        _quitButton.SetActive(false);
        _replayButton.SetActive(false);
        _menuButton.SetActive(false);
        ResumeGame();
        Over = false;
    }

    public void MainMenuButton()
    {
        MainMenu.load = true;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        gameObject.SetActive(false);
        _quitButton.SetActive(false);
        _replayButton.SetActive(false);
        _menuButton.SetActive(false);
        Over = false;
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
        _audioSource.Stop();
    }
}
