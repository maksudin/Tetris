using UnityEngine;

namespace Assets.Scripts
{
    public class CameraPositionManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _transforms;
        [SerializeField] private float _cameraSpeed;
        private Camera _camera;
        private Vector3 _movePos;
        private float _moveDistance;
        private bool _positionReached = true;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (_positionReached) return;

            var currentPos = _camera.transform.position;
            var resultVector = _movePos - currentPos;
            var direction = Vector3.Normalize(resultVector);
            float distance = Vector3.Distance(currentPos, _movePos);

            if (distance > 5f)
            {
                float n = Mathf.InverseLerp(1, _moveDistance, distance);
                var speed = _cameraSpeed * EaseInOutCubic(n);
                _camera.transform.Translate(new Vector3(direction.x * speed, 0, 0));
            }
            else
                _positionReached = true;
        }

        public void MoveCameraToTranform(int index)
        {
            _positionReached = false;
            _movePos = _transforms[index].position;
            _moveDistance = Vector3.Distance(_camera.transform.position, _movePos);
        }

        private float EaseInOutCubic(float x)
        {
            var xxx = x * x * x;
            return x < 0.5 ? 4 * xxx : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
        }
    }
}