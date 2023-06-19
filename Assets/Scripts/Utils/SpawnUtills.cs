using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class SpawnUtills
    {
        private const string _containerName = "###SPAWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, string containerName = _containerName)
        {
            var container = GameObject.Find(containerName);
            if (container == null)
                container = new GameObject(containerName);

            return Object.Instantiate(prefab, position, Quaternion.identity, container.transform);
        }

    }
}