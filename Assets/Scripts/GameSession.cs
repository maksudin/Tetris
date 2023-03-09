using System;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] public Score Score;
        public int CurrentLevel;
        public event Action OnLevelChange;
        private int _maxLevel;

        private void Awake()
        {
            _maxLevel = DefsFacade.I.LevelDef.MaximumLevel;
        }

        private void Start()
        {
            CurrentLevel = DefsFacade.I.LevelDef.GetLevelInfo(0).LevelValue;
            Score.OnScoreChange += CheckLevelThreshold;
            OnLevelChange?.Invoke();
        }

        private void OnDestroy()
        {
            Score.OnScoreChange -= CheckLevelThreshold;
        }

        public void ResetLevel()
        {
            CurrentLevel = DefsFacade.I.LevelDef.GetLevelInfo(0).LevelValue;
            OnLevelChange?.Invoke();
        }

        private void CheckLevelThreshold()
        {
            int levelThreshold = DefsFacade.I.LevelDef.GetLevelInfo(CurrentLevel).ScoreForNextLevel;
            if (Score.Value >= levelThreshold)
                LevelUp();
        }

        public void LevelUp()
        {
            if (CurrentLevel + 1 > _maxLevel)
                return;

            CurrentLevel++;
            OnLevelChange?.Invoke();
        }
    }
}