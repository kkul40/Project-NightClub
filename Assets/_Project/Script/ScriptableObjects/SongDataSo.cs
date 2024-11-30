using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "new Song Data")]
    public class SongDataSo : ScriptableObject
    {
        public List<AudioClip> Hiphop;
        public List<AudioClip> Dance;
        public List<AudioClip> Pop;
        public List<AudioClip> Temp;
    }
}