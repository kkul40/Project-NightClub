using System.Collections.Generic;
using UnityEngine;

namespace Disco_ScriptableObject
{
    [CreateAssetMenu(menuName = "New Music List/New Music List")]
    public class MusicListSO : ScriptableObject
    {
        public List<AudioClip> PopMusics;
        public List<AudioClip> RockMusics;
        public List<AudioClip> HipHopMusics;
    }
}