using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayMusic : MonoBehaviour
    {
        private AudioSource _source;

        private void Start()
        {
            _source = GameObject.FindGameObjectWithTag("AudioPlayer").GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_source.isPlaying)
            {
                var randomClip = MusicRandomizer.GetRandomClip();
                _source.PlayOneShot(randomClip);
            }
        }
    }
}