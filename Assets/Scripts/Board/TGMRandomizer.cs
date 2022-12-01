using UnityEngine;

namespace Assets.Scripts.Board
{
    public class TGMRandomizer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _tetrominoPrefabs;
        [SerializeField] private int _noOfTypes = 7;
        [SerializeField] private Shape[] _shapeHistory;
        [SerializeField] private int _tries = 4;
        private int _countTries;

        private void Awake()
        {
            _shapeHistory = new Shape[4]
            {
                Shape.Z, Shape.Z, Shape.Z, Shape.Z // TGM1
                //Shape.Z, Shape.Z, Shape.S, Shape.S // TGM2
            };
        }

        public GameObject GetRandomizedPrefab(bool isFirstTetromino = false)
        {
            int randomValue = RandomizeTetrominoType(isFirstTetromino);
            var tetrominoPrefab = _tetrominoPrefabs[randomValue];
            var nextShape = tetrominoPrefab.GetComponent<Tetromino>()._shape;

            foreach (var shape in _shapeHistory)
            {
                if (nextShape == shape && _countTries != _tries)
                {
                    _countTries++;
                    GetRandomizedPrefab(isFirstTetromino);
                }
                else
                {
                    _countTries = 0;
                    break;
                }
            }

            AddHistory(nextShape);

            return tetrominoPrefab;
        }

        private void AddHistory(Shape shape)
        {
            Shape[] _history = new Shape[4]
            {
                _shapeHistory[1],
                _shapeHistory[2],
                _shapeHistory[3],
                shape
            };

            _shapeHistory = _history;
        }

        private int RandomizeTetrominoType(bool isFirstTetromino)
        {
            if (isFirstTetromino)
                // The game never deals an S, Z or O as the first piece, to avoid a forced overhang.
                return (int)System.Math.Floor( Random.value * ( _noOfTypes - 3 ) );

            return (int) System.Math.Floor(Random.value * _noOfTypes);
        }
    }
}