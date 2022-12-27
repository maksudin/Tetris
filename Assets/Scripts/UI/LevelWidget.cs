using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LevelWidget : MonoBehaviour
    {
        [SerializeField] Text _levelText;
        [SerializeField] private Animator _animator;
        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _gameSession.OnLevelChange += PlayAnimation;
        }

        public void PlayAnimation()
        {
            _animator.SetTrigger("UpdateLevel");
        }

        public void UpdateLevel()
        {
            _levelText.text = _gameSession.CurrentLevel.ToString();
        }

        private void OnDestroy()
        {
            _gameSession.OnLevelChange -= UpdateLevel;
        }
    }
}