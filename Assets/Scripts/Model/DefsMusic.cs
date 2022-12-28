using UnityEngine;

namespace Assets.Scripts.Model
{
    [CreateAssetMenu(menuName = "Defs/DefsMusic", fileName = "DefsMusic")]
    public class DefsMusic : ScriptableObject
    {
        [SerializeField] private AudioClip[] _clips;

        public AudioClip[] Clips => _clips;

        private static DefsMusic _instance;
        public static DefsMusic I => _instance == null ? LoadDefs() : _instance;

        private static DefsMusic LoadDefs()
        {
            return _instance = Resources.Load<DefsMusic>("DefsMusic");
        }

        [ContextMenu("LoadMusic")]
        public void LoadMusic()
        {
            _clips = Resources.LoadAll<AudioClip>("Music");
        }
    }
}