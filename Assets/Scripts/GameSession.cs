using System;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] public Score Score;
        public int CurrentLevel;
        private int maxLevel;
        public event Action OnLevelUp;

        private void Awake()
        {
            maxLevel = DefsFacade.I.LevelDef.MaximumLevel;
        }

        private void Start()
        {
            CurrentLevel = DefsFacade.I.LevelDef.GetLevelInfo(0).LevelValue;
            Score.OnScoreChange += CheckLevelThreshold;
            OnLevelUp?.Invoke();
        }

        private void OnDestroy()
        {
            Score.OnScoreChange -= CheckLevelThreshold;
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
            OnLevelUp?.Invoke();
        }

        //private GameSession GetExistsSession()
        //{
        //    var sessions = FindObjectsOfType<GameSession>();
        //    foreach (var gameSession in sessions)
        //        if (gameSession != this)
        //            return gameSession;

        //    return null;
        //}
    }
}