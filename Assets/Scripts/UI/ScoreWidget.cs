using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ScoreWidget : MonoBehaviour
    {
        [SerializeField] Text _scoreText;
        private Score _score;

        private void Start()
        {
            _score = FindObjectOfType<Score>();
            _score.OnScoreChange += UpdateScore;
        }

        private void UpdateScore() => _scoreText.text = _score.Value.ToString();

        private void OnDestroy() => _score.OnScoreChange -= UpdateScore;
    }
}