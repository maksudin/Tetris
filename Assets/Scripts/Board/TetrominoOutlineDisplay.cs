using System;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class TetrominoOutlineDisplay : MonoBehaviour
    {
        [NonSerialized] public GameObject Prefab;



//#if UNITY_EDITOR
//        private void OnDrawGizmos()
//        {
//            var pos = transform.position;
//            Gizmos.color = Color.red;
//            DrawUtils.DrawRectangle(new Vector3[5]
//                {
//                    new Vector3(pos.x, pos.y, pos.z),
//                    new Vector3(pos.x + _displaySize.x, pos.y, pos.z),
//                    new Vector3(pos.x + _displaySize.x, pos.y + _displaySize.y, pos.z),
//                    new Vector3(pos.x, pos.y + _displaySize.y, pos.z),
//                    new Vector3(pos.x, pos.y, pos.z)
//                }
//            );
//            Gizmos.color = Color.white;
//        }
//#endif
    }
}