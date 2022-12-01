using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Board : MonoBehaviour
    {
        private GameObject _tetrominoPrefab;
        [SerializeField] private GameObject _blockedCellPrefab;
        [SerializeField] private Vector2 _boardSize = new Vector2(10, 20);

        private Tetromino Tetromino;
        private Vector2[] _TetrominoCoords;
        private Vector3 _tetrominoCenter;

        private Vector2 _direction;
        private int[,] _boardCells;
        private int _rotationCount;

        private bool _isRush;

        private BlockedCell[] _blockedCells;
        private TGMRandomizer _tGMRandomizer;

        private void Start()
        {
            _boardCells = new int[(int)_boardSize.x, (int)_boardSize.y];
            _tGMRandomizer = GetComponent<TGMRandomizer>();
            _tetrominoPrefab = _tGMRandomizer.GetRandomizedPrefab(isFirstTetromino: true);
            SpawnTetromino();

            _tetrominoCenter = new Vector3(1, 18, 0);
            SetTetrominoCoords(_tetrominoCenter);
        }

        private float _nextFallTime = 0.0f;
        [Range(0, 1)]
        [SerializeField] private float _fallSpeed = 0.1f;
        [SerializeField] private bool FallEnabled;

        private void Update()
        {
            if (!FallEnabled) return;
            if (Time.time > _nextFallTime)
            {
                _nextFallTime += _fallSpeed;
                Movement(new Vector2(0, -1));
            }
        }

        private void FindBlockedCellObjects()
        {
            _blockedCells = FindObjectsOfType<BlockedCell>();
        }

        private GameObject SpawnBlockedCell()
        {
            return SpawnUtills.Spawn(_blockedCellPrefab, Vector3.zero);
        }

        private void SpawnTetromino()
        {
            var spawned = SpawnUtills.Spawn(_tetrominoPrefab, Vector3.zero);
            Tetromino = spawned.GetComponent<Tetromino>();
        }

        public void Rush()
        {
            _isRush = true;
            Movement(new Vector2(0, -1));
        }

        public void RotateClockwise()
        {
            var rotationsLen = Tetromino.Rotations.Length;
            var rotationPieces = Tetromino.Rotations[_rotationCount % rotationsLen].Pieces;

            for (int i = 0; i < rotationPieces.Length; i++)
            {
                var X = rotationPieces[i].XPos + _TetrominoCoords[0].x;
                var Y = rotationPieces[i].YPos + _TetrominoCoords[0].y;

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
            if (_isRush) return;
            _direction = direction;
            if (FallEnabled) 
                Movement(new Vector2(_direction.x, 0));
            else
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
                    return;
            }

            for (int i = 0; i < _TetrominoCoords.Length; i++)
            {
                _TetrominoCoords[i].x += direction.x;
                _TetrominoCoords[i].y += direction.y;
            }

            Tetromino.transform.position = new Vector3(boardPos.x + direction.x, boardPos.y + direction.y, boardPos.z);

            if (_isRush)
                Movement(new Vector2(0, -1));
        }

        private void CleanUpAndPrepareNextTetromino()
        {
            BlockTetrominoCells();
            FindAndMoveBlockedRows();
            ResetSpawnedBlocks();
            Destroy(Tetromino.gameObject);
            _tetrominoPrefab = _tGMRandomizer.GetRandomizedPrefab(isFirstTetromino: false);
            SpawnTetromino();
            SetTetrominoCoords(_tetrominoCenter);
            _rotationCount = 0;
            _isRush = false;
        }

        private void ResetSpawnedBlocks()
        {
            FindBlockedCellObjects();
            for (int x = 0; x < _boardSize.x; x++)
                for (int y = 0; y < _boardSize.y; y++)
                {
                    var blockedCell = FindBlockedCell(x, y);
                    if (_boardCells[x, y] == 1 && blockedCell == null)
                    {
                        GameObject spawned = SpawnBlockedCell();
                        spawned.GetComponent<SpriteRenderer>().sprite = Tetromino.Sprite;
                        BlockedCell blocked = spawned.GetComponent<BlockedCell>();
                        blocked.Piece.XPos = x;
                        blocked.Piece.YPos = y;
                        spawned.transform.position = new Vector3(x + 0.5f, y + 0.5f, transform.position.z);
                        continue;
                    }
                    if (_boardCells[x, y] == 0 && blockedCell != null)
                        Destroy(blockedCell.gameObject);

                }
        }

        private BlockedCell FindBlockedCell(int x, int y)
        {
            foreach (var cell in _blockedCells)
                if (cell.Piece.XPos == x && cell.Piece.YPos == y)
                    return cell;

            return null;
        }

        private void BlockTetrominoCells()
        {
            foreach (var cell in _TetrominoCoords)
                _boardCells[(int)cell.x, (int)cell.y] = 1;
        }

        private void FindAndMoveBlockedRows()
        {
            for (int y = 0; y < _boardSize.y - 1; y++)
            {
                bool upperEdge = y == _boardSize.y - 1;
                bool isRowBlocked = IsRowBlocked(y);
                if (isRowBlocked && !upperEdge)
                {
                    RemoveBlockedRow(y);
                    for (int i = y; i < _boardSize.y - 1; i++)
                        MoveRowTo(fromIndex: i + 1, toIndex: i);

                    FindAndMoveBlockedRows();
                }

                else if (isRowBlocked)
                    RemoveBlockedRow(y);
            }
        }

        private void MoveRowTo(int fromIndex, int toIndex)
        {
            int[] row = GetBlockedRow(fromIndex);
            ReplaceRowWith(replaceIndex: toIndex, row);
            RemoveBlockedRow(fromIndex);
        }

        private void ReplaceRowWith(int replaceIndex, int[] rowVals)
        {
            for (int x = 0; x < _boardSize.x; x++)
                _boardCells[x, replaceIndex] = rowVals[x];
        }

        private int[] GetBlockedRow(int y)
        {
            int[] blockedRow = new int[(int)_boardSize.x];
            for (int x = 0; x < _boardSize.x; x++)
                blockedRow[x] = _boardCells[x, y];
            return blockedRow;
        }

        private bool IsRowBlocked(int y)
        {
            int countBlocked = 0;
            for (int x = 0; x < _boardSize.x; x++)
                if (_boardCells[x, y] == 1)
                    countBlocked++;

            return countBlocked == _boardSize.x ? true : false;
        }

        private void RemoveBlockedRow(int y)
        {
            for (int x = 0; x < _boardSize.x; x++)
            {
                _boardCells[x, y] = 0;
                //DestroyBlockedCell(x, y - 1);
            }
        }

        private void DestroyBlockedCell(int x, int y)
        {
            foreach (var cell in _blockedCells)
                if (cell.Piece.XPos == x && cell.Piece.YPos == y)
                    Destroy(cell.gameObject);
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



#if UNITY_EDITOR
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

#endif

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