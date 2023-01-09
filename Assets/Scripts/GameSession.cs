using System;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] public Score Score;
        public int CurrentLevel;
        private int maxLevel;
        public event Action OnLevelChange;

        private void Awake()
        {
            maxLevel = DefsFacade.I.LevelDef.MaximumLevel;
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
            if (CurrentLevel + 1 > maxLevel)
                return;

            CurrentLevel++;
            OnLevelChange?.Invoke();
        }
    }
}