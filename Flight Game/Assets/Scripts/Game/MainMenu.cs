using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _quitButton;

    [SerializeField]
    private GameObject _playButton;

    public void QuitGameButton() => Application.Quit();

    public void PlayGameButton()
    {
        SceneManager.LoadScene("Main Scene");
        gameObject.SetActive(false);
        _quitButton.SetActive(false);
        _playButton.SetActive(false);
    }
}
