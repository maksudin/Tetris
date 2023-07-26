using System;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Tetromino : MonoBehaviour
    {
        public Shape Shape;
        public Sprite Sprite, OutlineSprite;
        public SpriteMask _spriteMask;
        public bool _isMasked;
        [SerializeField] public Color Color;
        [SerializeField] private MaskPosition[] _maskPositions;
        public Rotations[] Rotations;
        public Piece[] Pieces;
        private Transform[] _piecesTransform;

        private void Awake()
        {
            _piecesTransform = GetComponentsInChildren<Transform>();
            _spriteMask.enabled = false;
            ApplySprites();
            RearrangePieces();
            SetMaskParams();
        }

        public void EnableMask() => _spriteMask.enabled = true;

        public void ApplySprites(bool isOutline = false)
        {
            var spriteRenders = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < 4; i++)
                if (isOutline)
                    spriteRenders[i].sprite = OutlineSprite;
                else
                    spriteRenders[i].sprite = Sprite;
        }

        public void RearrangePieces()
        {
            var pos = transform.position;
            for (int i = 0; i < _piecesTransform.Length - 2; i++)
                _piecesTransform[i + 1].position = new Vector3(Pieces[i].XPos + pos.x, Pieces[i].YPos + pos.y, pos.z);
        }

        public void SetMaskParams(int maskPositionNumber = 0)
        {
            var lScale = _spriteMask.transform.localScale;
            var y = _maskPositions[maskPositionNumber].Y - (lScale.y / 2) + 0.5f;
            //if (y == 0) y = (lScale.y / 2) - 0.5f;
            _spriteMask.transform.localPosition = new Vector3(_maskPositions[maskPositionNumber].X, y, 0);
            _spriteMask.transform.localScale = new Vector3(_maskPositions[maskPositionNumber].scaleX, lScale.y, lScale.z);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 pos = transform.position;

            foreach (var PieceCoord in Pieces)
                DrawUtils.DrawCell(pos, new Vector2(PieceCoord.XPos, PieceCoord.YPos));

            DrawRotations(pos);
            DrawMaskPositions(pos);
        }

        private void DrawRotations(Vector3 pos)
        {
            Gizmos.color = Color.red;
            foreach (var rotation in Rotations)
                foreach (var Piece in rotation.Pieces) 
                    if (rotation.Show)
                        DrawUtils.DrawCell(pos, new Vector2(Piece.XPos, Piece.YPos));

            Gizmos.color = Color.white;
        }

        private void DrawMaskPositions(Vector3 pos)
        {
            Gizmos.color = Color.yellow;
            foreach (var mask in _maskPositions)
                if (mask.Show)
                    DrawUtils.DrawMask(pos, new Vector2(mask.X, mask.Y), mask.scaleX);

            Gizmos.color = Color.white;
        }
#endif
    }

    [Serializable]
    public class Piece
    {
        public int XPos;
        public int YPos;
    }

    [Serializable]
    public class MaskPosition
    {
        public bool Show;
        public float X, Y;
        public float scaleX;
    }

    [Serializable]
    public class Rotations
    {
        public bool Show;
        public Piece[] Pieces;
    }

    public enum Shape
    {
        I,J,L,O,S,Z,T
    }
}