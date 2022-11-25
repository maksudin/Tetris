using System;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Tetromino : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] public Rotations[] Rotations;
        public Pieces[] Pieces;
        private Transform[] piecesTransform;

        private void Awake()
        {
            piecesTransform = GetComponentsInChildren<Transform>();   
            for (int i = 1; i < piecesTransform.Length; i++)
                piecesTransform[i].position = new Vector3(Pieces[i].XPos, Pieces[i].YPos, piecesTransform[i].position.z);
        }

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
    }

    [Serializable]
    public class Pieces
    {
        public int XPos;
        public int YPos;
    }

    [Serializable]
    public class Rotations
    {
        public bool Show;
        public Pieces[] Pieces;
    }
}