using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayMusic : MonoBehaviour
    {
        private AudioSource _musicSource;
        private AudioSource _SfxSource;

        private void Start()
        {
            _musicSource = GameObject.FindGameObjectWithTag("AudioPlayer").GetComponent<AudioSource>();
            _SfxSource = GameObject.FindGameObjectWithTag("SFXPlayer").GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_musicSource.isPlaying)
            {
                var randomClip = MusicRandomizer.GetRandomClip();
                _musicSource.PlayOneShot(randomClip);
            }
        }

        public void PlaySfxOfType(SfxType type)
        {
            var clip = SfxUtils.GetRandomSfxOfType(type);
            _SfxSource.PlayOneShot(clip);
        }
    }
}