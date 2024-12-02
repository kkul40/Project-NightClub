using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Song Data", menuName = "Data/Song Data")]
    public class SongDataSo : ScriptableObject
    {
        public List<SongData> Hiphop;
        public List<SongData> Dance;
        public List<SongData> Pop;
        public List<SongData> Temp;
    }

    [Serializable]
    public class SongData
    {
        public string Artist;
        public string SongName;
        public AudioClip Clip;
    }
}