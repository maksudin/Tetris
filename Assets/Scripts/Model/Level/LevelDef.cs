using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Model.Level
{
    [CreateAssetMenu(menuName = "Defs/LevelDef", fileName = "LevelDef")]
    public class LevelDef : ScriptableObject
    {
        [SerializeField] private LevelInfo[] _levelInfos;
        [SerializeField] private PointsForLines _points;

        public LevelInfo[] LevelInfos => _levelInfos;
        public PointsForLines Points => _points;

        public int MaximumLevel => LevelInfos.Last().LevelValue;

        public LevelInfo GetLevelInfo(int level)
        {
            foreach (var levelInfo in _levelInfos)
                if (levelInfo.LevelValue == level)
                    return levelInfo;

            return default;
        }

        public int GetPointsDefValueForLines(int lines, int level)
        {
            int points = 0;
            switch (lines)
            {
                case 1:
                    points =  _points.OneLine;
                    break;
                case 2:
                    points = _points.TwoLine;
                    break;
                case 3:
                    points = _points.ThreeLine;
                    break;
                case 4:
                    points = _points.FourLine;
                    break;
            }

            return points * (level + 1);
        }
    }

    [Serializable]
    public struct LevelInfo
    {
        [SerializeField] private int _levelValue;
        [SerializeField] private int _scoreForNextLevel;
        [SerializeField] private float _speed;

        public int LevelValue => _levelValue;
        public int ScoreForNextLevel => _scoreForNextLevel;
        public float Speed => _speed;
    }

    [Serializable]
    public struct PointsForLines
    {
        [SerializeField] private int oneLine;
        [SerializeField] private int twoLine;
        [SerializeField] private int threeLine;
        [SerializeField] private int fourLine;

        public int OneLine => oneLine;
        public int TwoLine => twoLine;
        public int ThreeLine => threeLine;
        public int FourLine => fourLine;
    }
}