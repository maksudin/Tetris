using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class MusicRandomizer
    {
        private List<AudioClip> _audioList;

        public static AudioClip GetRandomClip()
        {
            var audioClips = DefsMusic.I.Clips;
            var numOfClips = audioClips.Length;
            int randomNum = (int) System.Math.Floor(Random.value * numOfClips);
            return audioClips[randomNum - 1];
        }

        public AudioClip GetRandomClipFromList()
        {
            ReloadAudioList();

            var numOfClips = _audioList.Count;
            int randomNum = (int)System.Math.Floor(Random.value * numOfClips);
            var randomClip = _audioList[randomNum - 1];
            _audioList.RemoveAt(randomNum - 1);
            return randomClip;
        }

        private void ReloadAudioList()
        {
            if (_audioList.Count == 0)
            {
                var audioClips = DefsMusic.I.Clips;
                foreach (var clip in audioClips)
                    _audioList.Add(clip);
            }
        }
    }
}