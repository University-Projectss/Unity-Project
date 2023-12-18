using UnityEngine;

public class TerrainCrash : MonoBehaviour
{
    [SerializeField]
    private GameOver _gameOver;

    [SerializeField]
    private ScoreCounterSO _scoreCounterSO;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.TerrainTag) && !_gameOver.Over)
        {
            _scoreCounterSO.score.gameOver = "Crash";
            _scoreCounterSO.SaveScore();
            _gameOver.ShowGameOverCrash();
        }
    }
}