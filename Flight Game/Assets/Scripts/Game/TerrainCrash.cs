using System.Collections;
using UnityEngine;

public class TerrainCrash : MonoBehaviour
{
    [SerializeField]
    private GameOver _gameOver;

    [SerializeField]
    private ScoreCounterSO _scoreCounterSO;

    [SerializeField]
    [Range(0f, 1f)]
    private float _collisionLimit;

    private bool _collisionFlag = false;

    void OnCollisionEnter(Collision collision)
    {
        if (_collisionFlag || !collision.gameObject.CompareTag(Constants.TerrainTag) || _gameOver.Over)
        {
            return;
        }

        Vector3 planeSpeedVec = GetComponent<Rigidbody>().velocity;
        _collisionFlag = true;
        StartCoroutine(CrashCoroutine(planeSpeedVec));
    }

    private IEnumerator CrashCoroutine(Vector3 previousVelocity)
    {
        yield return new WaitForSeconds(0.05f);
        
        Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;

        if(currentVelocity.magnitude < previousVelocity.magnitude * (1 - _collisionLimit))
        {
            _scoreCounterSO.score.gameOver = "Crash";
            _scoreCounterSO.SaveScore();
            _gameOver.ShowGameOverCrash();
        }
        else
        {
            _collisionFlag = false;
        }
    }
}
