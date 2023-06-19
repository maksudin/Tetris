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
            var numOfClips = audioClips.Length - 1;
            int randomNum = (int) System.Math.Floor(Random.value * numOfClips);
            return audioClips[randomNum];
        }

        public AudioClip GetRandomClipFromList()
        {
            ReloadAudioList();

            var numOfClips = _audioList.Count - 1;
            int randomNum = (int)System.Math.Floor(Random.value * numOfClips);
            var randomClip = _audioList[randomNum];
            _audioList.RemoveAt(randomNum);
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