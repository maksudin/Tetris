using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector2 _boardSize = new Vector2(10, 20);

        private Tetromino Tetromino;
        private Vector2[] _TetrominoCoords;
        private Vector3 _tetrominoCenter;

        private Vector2 _direction;
        private int[,] _boardCells;
        private int _rotationCount;

        private void Awake()
        {
            //_boardSize = new Vector2(10, 20);
            _boardCells = new int[(int)_boardSize.x, (int)_boardSize.y];
            
            SpawnTetromino();

            _tetrominoCenter = new Vector3(1, 18, 0);
            SetTetrominoCoords(_tetrominoCenter);
        }

        private void SpawnTetromino()
        {
            var spawned = SpawnUtills.Spawn(prefab, Vector3.zero);
            Tetromino = spawned.GetComponent<Tetromino>();
        }

        public void RotateClockWise()
        {
            var rotationsLen = Tetromino.Rotations.Length;
            var rotationPieces = Tetromino.Rotations[_rotationCount % rotationsLen].Pieces;

            for (int i = 0; i < rotationPieces.Length; i++)
            {
                var X = rotationPieces[i].XPos + _TetrominoCoords[i].x;
                var Y = rotationPieces[i].YPos + _TetrominoCoords[i].y;

                if (IsCellOutOfBounds(coordX: X))
                    return;

                if (IsBottomReached(coordY: Y))
                    return;

                if (IsCellBlocked(coordX: X, coordY: Y))
                    return;

            }

            Tetromino.Pieces = Tetromino.Rotations[_rotationCount % rotationsLen].Pieces;
            Vector3 pos = Tetromino.transform.position;
            Vector3 tetrominoCoord = new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z); 
            SetTetrominoCoords(tetrominoCoord);
            Tetromino.RearrangePieces();

            _rotationCount++;
        }

        public void Move(Vector2 direction)
        {
            _direction = direction;
            Movement(_direction);
        }

        private void Movement(Vector2 direction)
        {
            if (Tetromino == null) return;
            var boardPos = Tetromino.transform.position;

            foreach (var coord in _TetrominoCoords)
            {
                var X = coord.x + direction.x;
                var Y = coord.y + direction.y;

                if (IsCellOutOfBounds(coordX: X))
                    return;

                if (IsBottomReached(coordY: Y))
                {
                    CleanUpAndPrepareNextTetromino();
                    return;
                }

                var isCellUnderTetromino = direction.x == 0 && direction.y == -1;

                if (IsCellBlocked(coordX: X, coordY: Y) && isCellUnderTetromino)
                {
                    CleanUpAndPrepareNextTetromino();
                    return;
                }
                else if (IsCellBlocked(coordX: X, coordY: Y))
                {
                    return;
                }
            }

            for (int i = 0; i < _TetrominoCoords.Length; i++)
            {
                _TetrominoCoords[i].x += direction.x;
                _TetrominoCoords[i].y += direction.y;
            }

            Tetromino.transform.position = new Vector3(boardPos.x + direction.x, boardPos.y + direction.y, boardPos.z);
        }

        private void CleanUpAndPrepareNextTetromino()
        {
            BlockTetrominoCells();
            Destroy(Tetromino.gameObject);
            SpawnTetromino();
            SetTetrominoCoords(_tetrominoCenter);
        }

        private void BlockTetrominoCells()
        {
            foreach (var cell in _TetrominoCoords)
                _boardCells[(int)cell.x, (int)cell.y] = 1;


            FindAndRemoveBlockedRows();
            // TODO: ¬се заблокированные клетки сдвинуть вниз.
        }

        private void FindAndRemoveBlockedRows()
        {
            int countBlocked;
            for (int y = 0; y < _boardSize.y; y++)
            {
                countBlocked = 0;
                for (int x = 0; x < _boardSize.x; x++)
                    if (_boardCells[x, y] == 1)
                        countBlocked++;

                if (countBlocked == _boardSize.x)
                    RemoveBlockedRow(y);
            }
        }

        private void RemoveBlockedRow(int y)
        {
            for (int x = 0; x < _boardSize.x; x++)
                _boardCells[x, y] = 0;
        }

        private bool IsCellOutOfBounds(float coordX)
        {
            return coordX > _boardSize.x - 1 || coordX < 0;
        }

        private bool IsBottomReached(float coordY)
        {
            return coordY < 0;
        }

        private bool IsCellBlocked(float coordX, float coordY)
        {
            return _boardCells[(int)coordX, (int)coordY] == 1;
        }

        private void OnDrawGizmos()
        {
            DrawUtils.DrawRectangle(new Vector3[5]
                {
                    new Vector3(0,0,0),
                    new Vector3(10,0,0),
                    new Vector3(10,20,0),
                    new Vector3(0,20,0),
                    new Vector3(0,0,0)
                }
            );

            Vector3 pos = transform.position;

            if (_TetrominoCoords.Length != 0)
            {
                // Tetromino
                foreach (var coord in _TetrominoCoords)
                    DrawUtils.DrawCell(pos, coord);
            }

            DrawBlockedCells(pos);
        }

        private void DrawBlockedCells(Vector3 pos)
        {
            Gizmos.color = Color.red;

            for (int x = 0; x < _boardSize.x; x++)
                for (int y = 0; y < _boardSize.y; y++)
                    if (_boardCells[x, y] == 1)
                        DrawUtils.DrawCell(pos, new Vector2(x, y));

            Gizmos.color = Color.white;
        }



        private void SetTetrominoCoords(Vector3 tetrominoCenter)
        {
            if (Tetromino == null) return;

            _TetrominoCoords = new Vector2[4]
            {
                new Vector2(Tetromino.Pieces[0].XPos + tetrominoCenter.x, Tetromino.Pieces[0].YPos + tetrominoCenter.y),
                new Vector2(Tetromino.Pieces[1].XPos + tetrominoCenter.x, Tetromino.Pieces[1].YPos + tetrominoCenter.y),
                new Vector2(Tetromino.Pieces[2].XPos + tetrominoCenter.x, Tetromino.Pieces[2].YPos + tetrominoCenter.y),
                new Vector2(Tetromino.Pieces[3].XPos + tetrominoCenter.x, Tetromino.Pieces[3].YPos + tetrominoCenter.y)
            };

            SetTetrominoPosition(tetrominoCenter);
        }

        private void SetTetrominoPosition(Vector3 center)
        {
            Tetromino.transform.position = new Vector3(
                Tetromino.Pieces[0].XPos + center.x + 0.5f,
                Tetromino.Pieces[0].YPos + center.y + 0.5f,
                center.z
            );
        }
    }
}