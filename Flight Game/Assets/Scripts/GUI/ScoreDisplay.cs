using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private ScoreCounterSO _scoreCounter;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    void Update()
    {
        _scoreText.text = _scoreCounter.score.total.ToString();
    }
}
