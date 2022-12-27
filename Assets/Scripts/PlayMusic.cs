using UnityEngine;

namespace Assets.Scripts
{
    public class PlayMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        private AudioSource _source;

        public void Play()
        {
            if (_source == null)
                _source = GameObject.FindObjectOfType<AudioSource>();

            _source.PlayOneShot(_clip);
        }
    }
}