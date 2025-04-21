using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace System.Character.NPC
{
    [CreateAssetMenu(menuName = "Data/Npc Animation Data")]
    public class NewAnimationSO : ScriptableObject
    {
        public float animationDuration;

        public AnimationClip Idle;
        public AnimationClip Walk;
        public AnimationClip Sit;
        public List<AnimationClip> LeanOnWall;
        public List<AnimationClip> Dance;

        public AvatarMask mask;
        [FormerlySerializedAs("Drink")] public AnimationClip drinkIdle;
        [FormerlySerializedAs("Drink")] public AnimationClip drinkAction;
        public AnimationClip Fight;

        public AnimAction test;
        

        public AnimationClip DebugAnim;
    }

    public class AnimAction
    {
        public AnimationClip idle;
        public AnimationClip action;
    }
}