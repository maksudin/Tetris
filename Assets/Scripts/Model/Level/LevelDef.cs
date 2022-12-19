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

        public int GetPointsDefValueForLines(int lines)
        {
            switch (lines)
            {
                case 1:
                    return _points.OneLine;
                case 2:
                    return _points.TwoLine;
                case 3:
                    return _points.ThreeLine;
                case 4:
                    return _points.FourLine;
                default:
                    return -1;
            }
        }
    }

    [Serializable]
    public struct LevelInfo
    {
        [SerializeField] private int levelValue;
        public int LevelValue => levelValue;
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