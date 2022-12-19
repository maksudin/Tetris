using System;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private int _value;
        public int Value => _value;

        public event Action OnScoreChange;

        private GameSession _gameSession;

        private void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();   
        }

        public void GetPointsForLines(int lines)
        {
            _value += DefsFacade.I.LevelDef.GetPointsDefValueForLines(lines);
            OnScoreChange?.Invoke();
        }

    }
}