using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn() => SpawnInstance();

        public GameObject SpawnInstance()
        {
            var instance = SpawnUtills.Spawn(_prefab, _target.position);
            var scale = _target.lossyScale;
            instance.transform.localScale = scale;
            instance.SetActive(true);
            return instance;
        }

        public void SpawnWithOffset(Vector2 offset)
        {
            Vector3 newPosition = new Vector3(_target.position.x + offset.x,
                                              _target.position.y + offset.y,
                                              _target.position.z);

            GameObject instantiate = SpawnUtills.Spawn(_prefab, newPosition);
            instantiate.transform.localScale = _target.lossyScale;
        }

        public void SpawnOnRandomPositionRange(Vector3 minPos, Vector3 maxPos, GameObject[] _prefubs)
        {

            foreach (GameObject go in _prefubs)
            {
                Vector3 newPosition = new Vector3(Random.Range(minPos.x, maxPos.x),
                                                  Random.Range(minPos.y, maxPos.y),
                                                  _target.position.z);
                GameObject instantiate = SpawnUtills.Spawn(_prefab, newPosition);
                instantiate.transform.localScale = _target.lossyScale;
            }
        }

        public void SetPrefub(GameObject prefab) => _prefab = prefab;
    }
}


