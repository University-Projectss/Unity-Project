using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGScoreInit : MonoBehaviour
{
    [SerializeField]
    private ScoreCounterSO _scoreCounter;
   
    //This is a temporary way to load/reset the score
    //After we create the main menu, the scores will be loaded when clicking start
    void Awake()
    {
        _scoreCounter.LoadScores();
    }

}
