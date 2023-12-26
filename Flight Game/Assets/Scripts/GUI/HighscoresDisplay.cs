using TMPro;
using UnityEngine;

public class HighscoresDisplay : MonoBehaviour
{
    [SerializeField]
    private ScoreCounterSO _scoreCounter;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public void WriteScores()
    {
        string text = "";
        foreach(var score in _scoreCounter.highscores.scores)
        {
            text += $"{score.total}\n";
        }

        _scoreText.text = text;
    }
}
