using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace System.Character.NPC
{
    [CreateAssetMenu(fileName = "new Npc Animation Set", menuName = "Data/Npc Animation Set")]
    public class NpcAnimationSo : ScriptableObject
    {
        public float animationDuration;

        [Title("Animations")] 
        public AnimationClip Idle;
        public AnimationClip Walk;
        public AnimationClip Sit;
        public List<AnimationClip> LeanOnWall;
        public List<AnimationClip> Dance;

        [Title("Action Animations")] 
        public ActionAnimation Drink;
        public ActionAnimation Fighh;

        [Title("Debug")] 
        public AnimationClip DebugAnim;
    }

    [Serializable]
    public class ActionAnimation
    {
        public AvatarMask Mask;
        public AnimationClip IdleClip;
        public AnimationClip ActionClip;
    }
}