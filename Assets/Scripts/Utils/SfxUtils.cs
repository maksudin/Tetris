using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class SfxUtils
    {
        public static AudioClip GetRandomSfxOfType(SfxType type)
        {
            SfxClipCategories[] sfxClipCats = DefsMusic.I.SFX;
            foreach (var category in sfxClipCats)
                if (category.Type == type)
                {
                    var numOfClips = category.Clips.Length - 1;
                    int randomNum = (int)System.Math.Floor(Random.value * numOfClips);
                    return category.Clips[randomNum];
                }

            return default;
        }
    }
}