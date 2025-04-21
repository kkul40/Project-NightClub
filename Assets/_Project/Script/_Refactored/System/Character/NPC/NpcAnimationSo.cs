using System.Collections.Generic;
using UnityEngine;

namespace System.Character.NPC
{
    [CreateAssetMenu(fileName = "new Npc Animation Set", menuName = "Data/Npc Animation Set")]
    public class NpcAnimationSo : ScriptableObject
    {
        [Serializable]
        public class ActionAnimation
        {
            public AvatarMask Mask;
            public AnimationClip IdleClip;
            public AnimationClip ActionClip;
        }
        
        public float animationDuration;

        public AnimationClip Idle;
        public AnimationClip Walk;
        public AnimationClip Sit;
        public List<AnimationClip> LeanOnWall;
        public List<AnimationClip> Dance;
        
        public ActionAnimation Drink;
        public ActionAnimation Fight;

        public AnimationClip DebugAnim;
    }
 
}