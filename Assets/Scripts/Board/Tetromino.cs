using System;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Tetromino : MonoBehaviour
    {
        [SerializeField] public Shape _shape;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public Rotations[] Rotations;
        public Piece[] Pieces;
        private Transform[] piecesTransform;

        private void Awake()
        {
            piecesTransform = GetComponentsInChildren<Transform>();
            ApplySprites();
            RearrangePieces();
        }

        private void ApplySprites()
        {
            var spriteRenders = GetComponentsInChildren<SpriteRenderer>();
            foreach (var render in spriteRenders)
                render.sprite = Sprite;
        }

        public void RearrangePieces()
        {
            var pos = transform.position;
            for (int i = 0; i < piecesTransform.Length - 1; i++)
                piecesTransform[i + 1].position = new Vector3(Pieces[i].XPos + pos.x, Pieces[i].YPos + pos.y, pos.z);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 pos = transform.position;

            foreach (var PieceCoord in Pieces)
                DrawUtils.DrawCell(pos, new Vector2(PieceCoord.XPos, PieceCoord.YPos));

            DrawRotations(pos);
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
#endif
    }

    [Serializable]
    public class Piece
    {
        public int XPos;
        public int YPos;
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