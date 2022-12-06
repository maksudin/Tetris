using System;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class NextTetrominoDisplay : MonoBehaviour
    {
        [SerializeField] private Vector2 _displaySize = new Vector2(7, 7);
        [SerializeField] private Vector3 _positionOffset;
        [NonSerialized] public GameObject Prefab;

        public void SetNextTetromino(GameObject nextPrefab)
        {
            var pos = transform.position;

            if (Prefab != null)
                Destroy(Prefab);

            var spawned = SpawnUtills.Spawn(nextPrefab, new Vector3(
                pos.x + _positionOffset.x, pos.y + _positionOffset.y, pos.z + _positionOffset.z)
            );
            Prefab = spawned;
        }

        private void OnDrawGizmos()
        {
            var pos = transform.position;
            Gizmos.color = Color.red;
            DrawUtils.DrawRectangle(new Vector3[5]
                {
                    new Vector3(pos.x, pos.y, pos.z),
                    new Vector3(pos.x + _displaySize.x, pos.y, pos.z),
                    new Vector3(pos.x + _displaySize.x, pos.y + _displaySize.y, pos.z),
                    new Vector3(pos.x, pos.y + _displaySize.y, pos.z),
                    new Vector3(pos.x, pos.y, pos.z)
                }
            );
            Gizmos.color = Color.white;
        }
    }
}