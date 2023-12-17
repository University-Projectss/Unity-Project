using System;
using UnityEngine;

public class TerrainCrash : MonoBehaviour
{
    [SerializeField]
    private GameOver _gameOver;

    [SerializeField]
    private float _collisionLimit;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(Constants.TerrainTag))
        {
            return;
        }

        Vector3 collisionDir = collision.contacts[0].point - transform.position;
        float inlineSpeed = Vector3.Dot(collisionDir.normalized, GetComponent<Rigidbody>().velocity);
        if (Math.Abs(inlineSpeed) < _collisionLimit)
        {
            _gameOver.ShowGameOverCrash();
        }
    }
}