using UnityEngine;

namespace Assets.Scripts
{
    public class CameraPositionManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform[] _transforms;
        Vector3 newPosition;

        private void Update()
        {
            Vector3 currentPosition = _camera.transform.position;
            //newPosition = _transforms[index].position;
        }

        public void MoveCameraToTranform(int index)
        {
            
        }
    }
}