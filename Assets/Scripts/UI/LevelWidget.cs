using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LevelWidget : MonoBehaviour
    {
        [SerializeField] Text _levelText;
        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _gameSession.OnLevelChange += UpdateLevel;
        }

        private void UpdateLevel()
        {
            _levelText.text = _gameSession.CurrentLevel.ToString();
        }

        private void OnDestroy()
        {
            _gameSession.OnLevelChange -= UpdateLevel;
        }
    }
}