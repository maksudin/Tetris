using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class DrawUtils
    {
        public static void DrawRectangle(Vector3[] coords)
        {
            for (int i = 0; i < coords.Length - 1; i++)
                Gizmos.DrawLine(coords[i], coords[i + 1]);
        }

        public static void DrawCell(Vector3 pos, Vector2 coord)
        {
            DrawRectangle(new Vector3[5]
            {
                new Vector3(pos.x - 0.5f + coord.x, pos.y + 0.5f + coord.y, 0),
                new Vector3(pos.x + 0.5f + coord.x, pos.y + 0.5f + coord.y, 0),
                new Vector3(pos.x + 0.5f + coord.x, pos.y - 0.5f + coord.y, 0),
                new Vector3(pos.x - 0.5f + coord.x, pos.y - 0.5f + coord.y, 0),
                new Vector3(pos.x - 0.5f + coord.x, pos.y + 0.5f + coord.y, 0)
            });
        }

        public static void DrawMask(Vector3 pos, Vector2 coord, float scaleX)
        {
            DrawRectangle(new Vector3[5]
            {
                new Vector3(pos.x - (0.5f * scaleX) + coord.x, pos.y + 0.5f + coord.y, 0),
                new Vector3(pos.x + (0.5f * scaleX) + coord.x, pos.y + 0.5f + coord.y, 0),
                new Vector3(pos.x + (0.5f * scaleX) + coord.x, pos.y - 0.5f + coord.y, 0),
                new Vector3(pos.x - (0.5f * scaleX) + coord.x, pos.y - 0.5f + coord.y, 0),
                new Vector3(pos.x - (0.5f * scaleX) + coord.x, pos.y + 0.5f + coord.y, 0)
            });
        }
    }
}