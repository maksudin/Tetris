using System;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameSession : MonoBehaviour
    {
        [NonSerialized] public Score Score;
        public int CurrentLevel;
        private int maxLevel;

        private void Awake()
        {
            Score = FindObjectOfType<Score>();
            maxLevel = DefsFacade.I.LevelDef.MaximumLevel;
        }

        public void LevelUp()
        {
            if (CurrentLevel + 1 > maxLevel)
                return;

            CurrentLevel++;
        }

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
                if (gameSession != this)
                    return gameSession;

            return null;
        }
    }
}