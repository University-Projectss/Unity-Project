using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTerrain : MonoBehaviour
{
    [SerializeField]
    private GameOver _gameOver;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(Constants.TerrainTag))
        {
            _gameOver.ShowGameOverCrash();
        }
    }
}