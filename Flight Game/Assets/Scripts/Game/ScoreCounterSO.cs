using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScoreCounter", menuName = "ScriptableObjects/ScoreCounter", order = 1)]
public class ScoreCounterSO : ScriptableObject
{
    [Serializable]
    public class Score
    {
        public int total = 0;
        public int time = 0;
        public int checkpoints = 0;
        public int targets = 0;
        public string gameOver = "";
    }

    public Score score;

    [SerializeField]
    private int _highScoreCount;

    [SerializeField]
    private string _fileName;
    
    [Serializable]
    private class HighScores
    {
        public List<Score> scores;
    }

    private HighScores _highscores;

    public void SaveScore()
    {

        score.total = score.time * 10 + score.checkpoints * 100 + score.targets * 250;
        _highscores.scores.Add(score);

        //This can be optimized by writing a custom comparer for SortedList
        _highscores.scores.Sort((s1, s2) => s1.total.CompareTo(s2.total));
        if (_highscores.scores.Count > _highScoreCount)
        {
            _highscores.scores.RemoveAt(10);
        }

        File.WriteAllText(_fileName, JsonUtility.ToJson(_highscores));
    }

    public void LoadScores()
    {
        Reset();
        _highscores = new();
        if (File.Exists(_fileName))
        {
           _highscores = JsonUtility.FromJson<HighScores>(File.ReadAllText(_fileName));
        }
    }

    public void Reset()
    {
        score = new Score();
    }
}
