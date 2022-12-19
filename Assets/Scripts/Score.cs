using System;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
    public class Score : MonoBehaviour
    {
        private int _value;
        public int Value => _value;

        public event Action OnScoreChange;

        public void AddScorePointsForLines(int lines, int level)
        {
            _value += DefsFacade.I.LevelDef.GetPointsDefValueForLines(lines, level);
            OnScoreChange?.Invoke();
        }

        public void ResetScore()
        {
            _value = 0;
            OnScoreChange?.Invoke();
        }
    }
}