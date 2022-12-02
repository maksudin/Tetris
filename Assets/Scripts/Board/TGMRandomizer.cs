using UnityEngine;

namespace Assets.Scripts.Board
{
    public class TGMRandomizer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _tetrominoPrefabs;
        [SerializeField] private int _tries = 4;
        [SerializeField] private Shape[] _shapeHistory;

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
            int randomValue;
            GameObject tetrominoPrefab = null;
            Shape nextShape = Shape.I;

            for (int countTries = 0; countTries < _tries; countTries++)
            {
                randomValue = RandomizeTetrominoType(isFirstTetromino);
                tetrominoPrefab = _tetrominoPrefabs[randomValue];
                nextShape = tetrominoPrefab.GetComponent<Tetromino>()._shape;

                if (!IsShapeInHistory(nextShape))
                    break;
            }

            AddShapeToHistory(nextShape);

            return tetrominoPrefab;
        }

        public bool IsShapeInHistory(Shape shape)
        {
            foreach (var shapeH in _shapeHistory)
                if (shape == shapeH)
                    return true;

            return false;
        }

        private void AddShapeToHistory(Shape shape)
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
            var numOfTypes = _tetrominoPrefabs.Length;
            if (isFirstTetromino)
                // The game never deals an S, Z or O as the first piece, to avoid a forced overhang.
                return (int)System.Math.Floor( Random.value * (numOfTypes - 3 ) );

            return (int) System.Math.Floor(Random.value * numOfTypes);
        }
    }
}