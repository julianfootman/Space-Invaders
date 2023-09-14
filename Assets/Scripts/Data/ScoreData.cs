using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data / Score Data", fileName = "Score Data")]
public class ScoreData : ScriptableObject
{
    public int _maxScores = 10;
    public List<ScoreInfo> _scoreInfos;

    public void AddNewScore(int score)
    {
        ScoreInfo info = new ScoreInfo();
        info._time = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
        info._score = score;

        if (_scoreInfos == null)
        {
            _scoreInfos = new List<ScoreInfo>();
        }

        bool isAdded = false;

        for (int i = 0 ; i < _scoreInfos.Count; i ++)
        {
            if (_scoreInfos[i]._score < score)
            {
                _scoreInfos.Insert(i, info);
                isAdded = true;
                break;
            }
        }

        if (isAdded == false)
        {
            _scoreInfos.Add(info);
        }

        if (_scoreInfos.Count > _maxScores)
        {
            _scoreInfos.RemoveAt(_scoreInfos.Count);
        }
    }

    [Serializable]
    public class ScoreInfo
    {
        public string _time;
        public int _score;
    }
}
