using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _quitButton;

    [SerializeField]
    private GameOver _gameover;

    [SerializeField]
    private GameObject _playButton;

    [SerializeField]
    private GameObject _easyMode;

    [SerializeField]
    private GameObject _casualMode;

    [SerializeField]
    private GameObject _normalMode;

    [SerializeField]
    private ScoreCounterSO _scoreCounter;
   
    public static bool load = true;

    private void Awake()
    {
        if (!load)
        {
            PlayGameButton();
        }
        else
        {
            // _gameover.ResumeGame();
        }

    }
        
    public void SwitchToCasual() => SceneManager.LoadScene("Casual Mode");
    public void SwitchToEasy() => SceneManager.LoadScene("Easy Mode");
    public void SwitchToNormal() => SceneManager.LoadScene("Main Scene");
    public void QuitGameButton() => Application.Quit();

    public void PlayGameButton()
    {
        _scoreCounter.LoadScores();
        gameObject.SetActive(false);
        _quitButton.SetActive(false);
        _playButton.SetActive(false);
        _casualMode.SetActive(false);
        _easyMode.SetActive(false);
        _normalMode.SetActive(false);
        _gameover.ResumeGame();
    }
}
