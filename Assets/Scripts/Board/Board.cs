using System;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Board
{
    [Serializable]
    public class SfxUnityEvent : UnityEvent<SfxType> { }

    public class Board : MonoBehaviour
    {
        [SerializeField] private GameObject _blockedCellPrefab;
        [SerializeField] private GameObject _lineClearPrefab;
        [SerializeField] private Vector2 _boardSize = new Vector2(10, 20);
        [SerializeField] private Transform _spawnPivot;
        private Vector3 _onEdgeSpawnPosition;
        private Vector3 _defaultSpawnPosition;
        private Vector3 _outlinePos;

        [Range(0, 1)]
        [SerializeField] private float _fallSpeed = 0.1f;
        [SerializeField] private float _softDropMultiplier = 1f;
        [SerializeField] private bool _fallEnabled;
        private float _nextFallTime = 0.0f;

        [SerializeField] public UnityEvent OnGameOver;
        [SerializeField] public UnityEvent OnRestart;
        [SerializeField] public UnityEvent OnPause;

        [SerializeField] public SfxUnityEvent OnLinesCleared;
        [SerializeField] public SfxUnityEvent OnPieceDestroyed;
        [SerializeField] public SfxUnityEvent OnGameOverSfx;

        private GameObject _tetrominoPrefab;
        private Tetromino _tetromino;
        private Tetromino _tetrominoOutline;

        private Vector2[] _tetrominoCoords;
        private Vector2[] _outlineCoords;
        private Vector2 _direction;

        private int[,] _boardCells;
        private Sprite[,] _boardCellsSprites;

        private int _rotationCount;
        private int _linesCleared;
        private int _clearLineCalls;

        private bool _isHardDrop;
        private bool _isPaused;
        private bool _isGameOver;
        private bool _isGameStarted;
        private bool _hardDropDisabled;

        private BlockedCell[] _blockedCells;
        private TGMRandomizer _tgmRandomizer;
        private NextTetrominoDisplay _nextTetrominoDisplay;

        private GameSession _gameSession;
        private Score _score;
        
        private void Start()
        {
            _boardCells = new int[(int)_boardSize.x, (int)_boardSize.y];
            _boardCellsSprites = new Sprite[(int)_boardSize.x, (int)_boardSize.y];
            _tgmRandomizer = GetComponent<TGMRandomizer>();
            _gameSession = FindObjectOfType<GameSession>();
            _score = FindObjectOfType<Score>();
            _gameSession.OnLevelChange += OnLevelUp;
            _defaultSpawnPosition = _spawnPivot.position;
            _onEdgeSpawnPosition = new Vector3(_spawnPivot.position.x, _spawnPivot.position.y + 1, _spawnPivot.position.z);

            SetDefaultSpeed();
        }

        private void Update()
        {
            UpdateOutlinePosition();
            if (Time.time > _nextFallTime)
            {
                _nextFallTime += _fallSpeed * _softDropMultiplier;
                if (_isPaused || !_isGameStarted || !_fallEnabled) return;
                Movement(new Vector2(0, -1));
            }
        }

        private void OnDestroy()
        {
            _gameSession.OnLevelChange -= OnLevelUp;
        }

        public void PauseGame()
        {
            if (_isGameOver || !_isGameStarted) return; 

            _isPaused = true;
            ControlsUtils.DisableInput();
            OnPause?.Invoke();
        }

        public void UnPauseGame()
        {
            _isPaused = false;
            ControlsUtils.EnableInput();
        }

        public void StartGame()
        {
            _isGameOver = false;
            _isGameStarted = true;

            _tetrominoPrefab = _tgmRandomizer.GetRandomizedPrefab(isFirstTetromino: true);
            _nextTetrominoDisplay = FindObjectOfType<NextTetrominoDisplay>();

            UpdateNextTetrominoDisplay();
            SpawnTetromino();
            SpawnOutline();
            SetTetrominoCoords(_spawnPivot.position);
            SetOutlineCoords(_spawnPivot.position);
            UpdateOutlinePosition();
        }

        public void Restart()
        {
            RemoveAllBlockedCells();

            _isHardDrop = false;

            _tgmRandomizer.ResetHistory();
            if (_tetromino != null)
                Destroy(_tetromino.gameObject);
            if (_tetrominoOutline != null)
                Destroy(_tetrominoOutline.gameObject);

            StartGame();
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
            GameObject spawned;
            if (RowHasBlocks((int)_boardSize.y - 4))
                _spawnPivot.position = _onEdgeSpawnPosition;
            else
                _spawnPivot.position = _defaultSpawnPosition;

            spawned = SpawnUtills.Spawn(_tetrominoPrefab, _spawnPivot.position);
            _tetromino = spawned.GetComponent<Tetromino>();
        }

        public void HardDrop()
        {
            if (_hardDropDisabled) return;
            _isHardDrop = true;
            Movement(new Vector2(0, -1));
        }

        public void RotateTetromino(bool isClockwise = true)
        {
            if (_rotationCount == 0 && isClockwise == false)
                _rotationCount = _tetromino.Rotations.Length - 1;
            else if (!isClockwise)
                _rotationCount--;
            else
                _rotationCount++;

            var rotationsLen = _tetromino.Rotations.Length;
            var rotationPieces = _tetromino.Rotations[_rotationCount % rotationsLen].Pieces;

            for (int i = 0; i < rotationPieces.Length; i++)
            {
                var X = rotationPieces[i].XPos + _tetrominoCoords[0].x;
                var Y = rotationPieces[i].YPos + _tetrominoCoords[0].y;

                if (IsCellOutOfBounds(coordX: X))
                    return;

                if (IsBottomReached(coordY: Y))
                    return;

                if (IsCellBlocked(coordX: X, coordY: Y))
                    return;

            }

            _tetromino.Pieces = _tetromino.Rotations[_rotationCount % rotationsLen].Pieces;
            Vector3 pos = _tetromino.transform.position;
            Vector3 tetrominoCoord = new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z); 
            SetTetrominoCoords(tetrominoCoord);
            _tetromino.RearrangePieces();

            RotateOutline();
        }

        private void RotateOutline()
        {
            var rotationsLen = _tetromino.Rotations.Length;

            _tetrominoOutline.Pieces = _tetromino.Rotations[_rotationCount % rotationsLen].Pieces;
            _tetrominoOutline.RearrangePieces();
            ResetOutlinePosition();
        }

        public void Move(Vector2 direction)
        {
            if (_isHardDrop) return;
            _direction = direction;

            if (_fallEnabled) 
                Movement(new Vector2(_direction.x, 0));
            else
                Movement(_direction);
        }

        private void Movement(Vector2 direction)
        {
            if (_tetromino == null) return;
            var boardPos = _tetromino.transform.position;

            foreach (var coord in _tetrominoCoords)
            {
                var X = coord.x + direction.x;
                var Y = coord.y + direction.y;

                if (IsCellOutOfBounds(coordX: X))
                    return;

                if (IsBottomReached(coordY: Y))
                {
                    PrepareNextTetromino();
                    return;
                }

                if (CheckUpperRowReached())
                {
                    PrepareNextTetromino();
                    return;
                }

                var isCellUnderTetromino = direction.x == 0 && direction.y == -1;

                if (IsCellBlocked(coordX: X, coordY: Y) && isCellUnderTetromino)
                {
                    PrepareNextTetromino();
                    return;
                }
                else if (IsCellBlocked(coordX: X, coordY: Y))
                    return;
            }

            for (int i = 0; i < _tetrominoCoords.Length; i++)
            {
                _tetrominoCoords[i].x += direction.x;
                _tetrominoCoords[i].y += direction.y;
            }

            _tetromino.transform.position = new Vector3(boardPos.x + direction.x, boardPos.y + direction.y, boardPos.z);

            if (_isHardDrop)
                Movement(new Vector2(0, -1));
        }

        private void SpawnOutline()
        {
            var spawned = SpawnUtills.Spawn(_tetrominoPrefab, _spawnPivot.position);
            _tetrominoOutline = spawned.GetComponent<Tetromino>();
            _tetrominoOutline.ApplySprites(isOutline: true);
            ResetOutlinePosition();
        }

        private void ResetOutlinePosition()
        {
            Vector3 pos = _tetromino.transform.position;
            Vector3 tetrominoCoord = new Vector3((float)Math.Floor(pos.x), (float)Math.Floor(pos.y), pos.z);
            Vector3 outlineCoord = new Vector3(tetrominoCoord.x + 0.5f, tetrominoCoord.y + 0.5f, tetrominoCoord.z);

            SetOutlineCoords(tetrominoCoord);
            _outlinePos = outlineCoord;
        }

        private void UpdateOutlinePosition()
        {
            if (_tetrominoOutline == null) return;
            var direction = new Vector2(0, -1);
            var tetrominoPos = _tetromino.transform.position;

            if (tetrominoPos.x != _tetrominoOutline.transform.position.x)
                ResetOutlinePosition();

            while (true)
            {
                foreach (var coord in _outlineCoords)
                {
                    var X = coord.x + direction.x;
                    var Y = coord.y + direction.y;
                    if (IsCellOutOfBounds(coordX: X))
                        return;
                    if (IsBottomReached(coordY: Y) || IsCellBlocked(coordX: X, coordY: Y))
                        return;
                }

                for (int i = 0; i < _outlineCoords.Length; i++)
                {
                    _outlineCoords[i].x += direction.x;
                    _outlineCoords[i].y += direction.y;
                }

                var newOutlinePos = new Vector3(_outlinePos.x, _outlinePos.y + direction.y, _outlinePos.z);

                _tetrominoOutline.transform.position = newOutlinePos;
                _outlinePos = newOutlinePos;
            }
        }
        
        private void PrepareNextTetromino()
        {
            BlockTetrominoCells();
            FindBlockedCellObjects();
            RemoveBlockedRows();
            if (_linesCleared > 0)
            {
                _score.AddScorePointsForLines(_linesCleared, _gameSession.CurrentLevel);
                _hardDropDisabled = true;
            }

            Destroy(_tetromino.gameObject);
            Destroy(_tetrominoOutline.gameObject);
            OnPieceDestroyed?.Invoke(SfxType.Blocked);

            _rotationCount = 0;
            _isHardDrop = false;
            _linesCleared = 0;

            if (CheckUpperRowReached())
            {
                OnGameOver?.Invoke();
                OnGameOverSfx?.Invoke(SfxType.Lost);
                _isGameOver = true;
                return;
            }

            _tetrominoPrefab = _nextTetrominoDisplay.Prefab;

            UpdateNextTetrominoDisplay();
            SpawnTetromino();
            SpawnOutline();
            SetTetrominoCoords(_spawnPivot.position);
            SetOutlineCoords(_spawnPivot.position);
        }

        private void RemoveBlockedRows()
        {
            for (int y = 0; y < _boardSize.y - 1; y++)
            {
                bool upperEdge = y == _boardSize.y - 1;
                if (IsRowBlocked(y) && !upperEdge)
                {
                    var sprites = GetBlockedRowSprites(y);
                    RemoveBlockedRow(y);
                    var spawned = SpawnLineClear(y);
                    spawned.GetComponent<ClearLineController>().SetLineSprites(sprites);

                    _linesCleared++;
                    OnLinesCleared?.Invoke(SfxType.Cleared);
                }
            }

            _clearLineCalls = _linesCleared;
        }

        private void RearrangeRows()
        {
            for (int y = 0; y < _boardSize.y - 1; y++)
                if (IsRowEmpty(y) && RowHasBlocks(y + 1))
                {
                    SwapRows(y, y + 1);
                    SwapRowsSprites(y, y + 1);
                    RearrangeRows();
                    break;
                }
        }

        private void DestroyAllBlockedCells()
        {
            FindBlockedCellObjects();

            foreach (var cell in _blockedCells)
                Destroy(cell.gameObject);
        }

        public void DestroyRedundantCells()
        {
            _clearLineCalls -= 1;
            if (_clearLineCalls != 0) return;

            for (int x = 0; x < _boardSize.x; x++)
                for (int y = 0; y < _boardSize.y; y++)
                {
                    var blockedCell = FindBlockedCell(x, y);
                    if (_boardCells[x, y] == 0 && blockedCell != null)
                        Destroy(blockedCell.gameObject);
                }
        }

        //public void MoveBlockedCellsDown()
        //{
        //    if (_clearLineCalls != 0) return;
        //    FindBlockedCellObjects();

        //    for (int x = 0; x < _boardSize.x; x++)
        //        for (int y = 0; y < _boardSize.y; y++)
        //        {
        //            var blockedCell = FindBlockedCell(x, y);

        //            if (_boardCells[x, y] == 1 && blockedCell != null)
        //            {
        //                var nYPos = blockedCell.Piece.YPos - _moveCount;
        //                if (nYPos < 0) continue;
        //                blockedCell.Piece.YPos = nYPos;
        //                blockedCell.SetPosition();
        //            }
        //        }

        //    RearrangeRows();
        //    _moveCount = 0;
        //    _hardDropDisabled = false;
        //}

        public void RearrangeBlockedCells()
        {
            if (_clearLineCalls != 0) return;
            RearrangeRows();
            FindBlockedCellObjects();
            List<BlockedCell> freeBlockedCells = GetFreeBlockedCells();
            List<Vector2> boardCellCoords = GetAllBlockedBoardCellCoords();
            for (int i = 0; i < freeBlockedCells.Count; i++)
            {

                freeBlockedCells[i].Piece.XPos = (int)boardCellCoords[i].x;
                freeBlockedCells[i].Piece.YPos = (int)boardCellCoords[i].y;
                freeBlockedCells[i].SetSprite(_boardCellsSprites[(int)boardCellCoords[i].x, (int)boardCellCoords[i].y]);
                freeBlockedCells[i].SetPosition();
            }
            
            _hardDropDisabled = false;
        }

        private List<BlockedCell> GetFreeBlockedCells()
        {
            var freeBls = new List<BlockedCell>();
            foreach (var bl in _blockedCells)
                if (bl.IsVisible)
                    freeBls.Add(bl);

            return freeBls;
        }

        private List<Vector2> GetAllBlockedBoardCellCoords()
        {
            var coords = new List<Vector2>();
            for (int x = 0; x < _boardSize.x; x++)
                for (int y = 0; y < _boardSize.y; y++)
                    if (_boardCells[x, y] == 1)
                        coords.Add(new Vector2(x, y));

            return coords;
        }

        //public void CreateMissingBlockedCells()
        //{
        //    FindBlockedCellObjects();
        //    for (int x = 0; x < _boardSize.x; x++)
        //        for (int y = 0; y < _boardSize.y; y++)
        //        {
        //            var blockedCell = FindBlockedCell(x, y);

        //            if (_boardCells[x, y] == 1 && blockedCell == null)
        //            {
        //                GameObject spawned = SpawnBlockedCell();
        //                spawned.GetComponent<SpriteRenderer>().sprite = _tetromino.Sprite;
        //                BlockedCell blocked = spawned.GetComponent<BlockedCell>();
        //                blocked.Piece.XPos = x;
        //                blocked.Piece.YPos = y;
        //                blocked.SetPosition();
        //                continue;
        //            }

        //            else if (_boardCells[x, y] == 1 && blockedCell != null && !blockedCell.IsVisible)
        //                blockedCell.SetVisibility(true);
        //        }
        //}

        private GameObject SpawnLineClear(int y)
        {
            var spawned = SpawnUtills.Spawn(_lineClearPrefab, _spawnPivot.position);
            var boardPos = _tetromino.transform.position;
            spawned.transform.position = new Vector3(0, y, boardPos.z);
            return spawned;
        }

        private void SwapRows(int row1, int row2)
        {
            int[] row1Vals = new int[(int)_boardSize.x];
            int[] row2Vals = new int[(int)_boardSize.x];

            for (int x = 0; x < _boardSize.x; x++)
            {
                row1Vals[x] = _boardCells[x, row1];
                row2Vals[x] = _boardCells[x, row2];
            }

            for (int x = 0; x < _boardSize.x; x++)
            {
                _boardCells[x, row1] = row2Vals[x];
                _boardCells[x, row2] = row1Vals[x];
            }
        }

        private void SwapRowsSprites(int row1, int row2)
        {
            Sprite[] row1Sprites = new Sprite[(int)_boardSize.x];
            Sprite[] row2Sprites = new Sprite[(int)_boardSize.x];

            for (int x = 0; x < _boardSize.x; x++)
            {
                row1Sprites[x] = _boardCellsSprites[x, row1];
                row2Sprites[x] = _boardCellsSprites[x, row2];
            }

            for (int x = 0; x < _boardSize.x; x++)
            {
                _boardCellsSprites[x, row1] = row2Sprites[x];
                _boardCellsSprites[x, row2] = row1Sprites[x];
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

        private bool RowHasBlocks(int y)
        {
            for (int x = 0; x < _boardSize.x; x++)
                if (_boardCells[x, y] == 1)
                    return true;

            return false;
        }

        private bool IsRowBlocked(int y)
        {
            int countBlocked = 0;
            for (int x = 0; x < _boardSize.x; x++)
                if (_boardCells[x, y] == 1)
                    countBlocked++;

            return countBlocked == _boardSize.x;
        }

        private bool IsRowEmpty(int y)
        {
            int countEmpty = 0;
            for (int x = 0; x < _boardSize.x; x++)
                if (_boardCells[x, y] == 0)
                    countEmpty++;

            return countEmpty == _boardSize.x;
        }

        private void RemoveBlockedRow(int y)
        {
            for (int x = 0; x < _boardSize.x; x++)
                _boardCells[x, y] = 0;

            FindBlockedCellObjects();

            for (int x = 0; x < _boardSize.x; x++)
            {
                var blockedCell = FindBlockedCell(x, y);
                if (blockedCell != null)
                    blockedCell.SetVisibility(false);
            }
        }

        private Sprite[] GetBlockedRowSprites(int y)
        {
            Sprite[] sprites = new Sprite[(int)_boardSize.x];
            for (int x = 0; x < _boardSize.x; x++)
            {
                var blockedCell = FindBlockedCell(x, y);
                if (blockedCell != null)
                    sprites[x] = blockedCell.GetComponent<SpriteRenderer>().sprite;
            }

            return sprites;
        }

        private void RemoveAllBlockedCells()
        {
            for (int x = 0; x < _boardSize.x; x++)
                for (int y = 0; y < _boardSize.y; y++)
                    _boardCells[x, y] = 0;

            DestroyAllBlockedCells();
        }

        private void UpdateNextTetrominoDisplay()
        {
            var nextPrefab = _tgmRandomizer.GetRandomizedPrefab();
            _nextTetrominoDisplay.SetNextTetromino(nextPrefab);
        }

        private bool CheckUpperRowReached()
        {
            for (int x = 0; x < _boardSize.x; x++)
                if (_boardCells[x, (int)_boardSize.y - 1] == 1)
                    return true;

            return false;
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
            foreach (var cell in _tetrominoCoords)
            {
                var x = (int)cell.x;
                var y = (int)cell.y;
                if (x > _boardSize.x || y > _boardSize.y - 1) 
                    return;
                _boardCells[x, y] = 1;

                GameObject spawned = SpawnBlockedCell();
                spawned.GetComponent<SpriteRenderer>().sprite = _tetromino.Sprite;
                _boardCellsSprites[x, y] = _tetromino.Sprite;

                BlockedCell blocked = spawned.GetComponent<BlockedCell>();
                blocked.Piece.XPos = x;
                blocked.Piece.YPos = y;
                spawned.transform.position = new Vector3(x + 0.5f, y + 0.5f, transform.position.z);
                blocked.PlayAnimation();
                continue;
            }
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
            Gizmos.color = Color.red;
            DrawUtils.DrawRectangle(new Vector3[5]
                {
                    new Vector3(0,0,0),
                    new Vector3(_boardSize.x,0,0),
                    new Vector3(_boardSize.x,_boardSize.y,0),
                    new Vector3(0,_boardSize.y,0),
                    new Vector3(0,0,0)
                }
            );


            Gizmos.color = Color.white;


            //Vector3 pos = transform.position;

            //if (_tetrominoCoords.Length != 0)
            //{
            //    // Tetromino
            //    foreach (var coord in _tetrominoCoords)
            //        DrawUtils.DrawCell(pos, coord);
            //}

            //DrawBlockedCells(pos);
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
            if (_tetromino == null) return;

            _tetrominoCoords = new Vector2[4]
            {
                new Vector2(_tetromino.Pieces[0].XPos + tetrominoCenter.x, _tetromino.Pieces[0].YPos + tetrominoCenter.y),
                new Vector2(_tetromino.Pieces[1].XPos + tetrominoCenter.x, _tetromino.Pieces[1].YPos + tetrominoCenter.y),
                new Vector2(_tetromino.Pieces[2].XPos + tetrominoCenter.x, _tetromino.Pieces[2].YPos + tetrominoCenter.y),
                new Vector2(_tetromino.Pieces[3].XPos + tetrominoCenter.x, _tetromino.Pieces[3].YPos + tetrominoCenter.y)
            };

            SetTetrominoPosition(tetrominoCenter);
        }

        private void SetOutlineCoords(Vector3 outlineCenter)
        {
            if (_tetrominoOutline == null) return;

            _outlineCoords = new Vector2[4]
            {
                new Vector2(_tetromino.Pieces[0].XPos + outlineCenter.x, _tetromino.Pieces[0].YPos + outlineCenter.y),
                new Vector2(_tetromino.Pieces[1].XPos + outlineCenter.x, _tetromino.Pieces[1].YPos + outlineCenter.y),
                new Vector2(_tetromino.Pieces[2].XPos + outlineCenter.x, _tetromino.Pieces[2].YPos + outlineCenter.y),
                new Vector2(_tetromino.Pieces[3].XPos + outlineCenter.x, _tetromino.Pieces[3].YPos + outlineCenter.y)
            };

            SetOutlinePosition(outlineCenter);
        }

        private void SetTetrominoPosition(Vector3 center)
        {
            _tetromino.transform.position = new Vector3(
                _tetromino.Pieces[0].XPos + center.x + 0.5f,
                _tetromino.Pieces[0].YPos + center.y + 0.5f,
                center.z
            );
        }

        private void SetOutlinePosition(Vector3 center)
        {
            _tetrominoOutline.transform.position = new Vector3(
                _tetrominoOutline.Pieces[0].XPos + center.x + 0.5f,
                _tetrominoOutline.Pieces[0].YPos + center.y + 0.5f,
                center.z
            );
        }

        public void OnExitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void OnLevelUp()
        {
            ChangeSpeed();
            //var clip = SfxUtils.GetRandomSfxOfType(SfxType.LevelUp);
            //SfxUtils.PlaySfx(clip);
        }

        private void ChangeSpeed()
        {
            var defSpeed = DefsFacade.I.LevelDef.GetLevelInfo(_gameSession.CurrentLevel).Speed;
            _fallSpeed = defSpeed;
        }

        private void SetDefaultSpeed()
        {
            var defSpeed = DefsFacade.I.LevelDef.GetLevelInfo(0).Speed;
            _fallSpeed = defSpeed;
        }

        public void EnableControls()
        {
            ControlsUtils.EnableInput();
        }
    }
}