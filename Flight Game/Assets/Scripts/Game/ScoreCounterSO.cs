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
        public int portals = 0;
        public string gameOver = "";
    }

    public Score score;

    [SerializeField]
    private int _highScoreCount;

    [SerializeField]
    private string _fileName;
    
    [Serializable]
    public class HighScores
    {
        public List<Score> scores;
    }

    public HighScores highscores;

    public void SaveScore()
    {
        highscores.scores.Add(score);

        //This can be optimized by writing a custom comparer for SortedList
        highscores.scores.Sort((s1, s2) => s2.total.CompareTo(s1.total));
        if (highscores.scores.Count > _highScoreCount)
        {
            highscores.scores.RemoveAt(_highScoreCount);
        }

        File.WriteAllText(_fileName, JsonUtility.ToJson(highscores));
    }

    public void LoadScores()
    {
        Reset();
        highscores = new();
        if (File.Exists(_fileName))
        {
           highscores = JsonUtility.FromJson<HighScores>(File.ReadAllText(_fileName));
        }
    }

    public void Reset()
    {
        score = new Score();
    }
}
