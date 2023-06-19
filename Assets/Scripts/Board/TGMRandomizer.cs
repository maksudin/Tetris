using UnityEngine;

namespace Assets.Scripts.Board
{
    public class TGMRandomizer : MonoBehaviour
    {
        [SerializeField] private bool _debug;
        [SerializeField] private GameObject[] _tetrominoPrefabs;
        [SerializeField] private int _tries = 4;
        [SerializeField] private Shape[] _shapesHistory;

        private void Awake() => ResetHistory();

        public void ResetHistory()
        {
            _shapesHistory = new Shape[4]
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
            if (_debug)
            {
                tetrominoPrefab = _tetrominoPrefabs[0];
                return tetrominoPrefab;
            }
                

            for (int countTries = 0; countTries < _tries; countTries++)
            {
                randomValue = RandomizeTetrominoType(isFirstTetromino);
                tetrominoPrefab = _tetrominoPrefabs[randomValue];
                nextShape = tetrominoPrefab.GetComponent<Tetromino>().Shape;

                if (!IsShapeInHistory(nextShape))
                    break;
            }

            AddShapeToHistory(nextShape);

            return tetrominoPrefab;
        }

        public bool IsShapeInHistory(Shape shape)
        {
            foreach (var shapeH in _shapesHistory)
                if (shape == shapeH)
                    return true;

            return false;
        }

        private void AddShapeToHistory(Shape shape)
        {
            Shape[] _history = new Shape[4]
            {
                _shapesHistory[1],
                _shapesHistory[2],
                _shapesHistory[3],
                shape
            };

            _shapesHistory = _history;
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