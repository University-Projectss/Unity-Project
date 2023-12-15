using UnityEngine;

public class TerrainCrash : MonoBehaviour
{
    [SerializeField]
    private GameOver _gameOver;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.TerrainTag))
        {
            _gameOver.ShowGameOverCrash();
        }
    }
}