using System;
using UnityEngine;

public class TerrainCrash : MonoBehaviour
{
    [SerializeField]
    private GameOver _gameOver;

    [SerializeField]
    private ScoreCounterSO _scoreCounterSO;

    [SerializeField]
    private float _collisionLimit;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(Constants.TerrainTag) || _gameOver.Over)
        {
            return;
        }

        Vector3 planeSpeedVec = GetComponent<Rigidbody>().velocity;
        float avg = 0;
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 collisionDir = contact.point - transform.position;
            avg += Vector3.Dot(collisionDir.normalized, planeSpeedVec);
        }

        if (Math.Abs(avg / collision.contacts.Length) < _collisionLimit)
        {
            _scoreCounterSO.score.gameOver = "Crash";
            _scoreCounterSO.SaveScore();
            _gameOver.ShowGameOverCrash();
        }
    }
}
