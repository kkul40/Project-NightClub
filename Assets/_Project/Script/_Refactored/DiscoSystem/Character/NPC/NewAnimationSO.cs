using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiscoSystem.Character.NPC
{
    [CreateAssetMenu(menuName = "Data/Npc Animation Data")]
    public class NewAnimationSO : ScriptableObject
    {
        public float animationDuration;

        [Header("Animations")]
        public AnimationClip Idle;
        public AnimationClip Walk;
        public AnimationClip Sit;
        public List<AnimationClip> LeanOnWall;
        public List<AnimationClip> Dance;

        [Header("Action Animations")] 
        public ActionAnimation DrinkBeer;

        public AnimationClip DebugAnim;
    }

    [Serializable]
    public class ActionAnimation
    {
        public AvatarMask Mask;
        public AnimationClip Idle;
        public AnimationClip Action;
    }
}