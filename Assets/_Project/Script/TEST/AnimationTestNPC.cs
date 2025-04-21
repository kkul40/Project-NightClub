using System.Character.NPC;
using Animancer;
using Data;
using UnityEngine;

namespace DefaultNamespace.TEST
{
    public class AnimationTestNPC : MonoBehaviour
    {
        public IAnimationController AnimationController { get; private set; }
        
        public void Initialize(NpcAnimationSo npcAnimationSo, Animator animator, AnimancerComponent animancerComponent, Transform armatureTransform)
        {
            AnimationController = new NPCAnimationControl(animator, animancerComponent, npcAnimationSo, armatureTransform);
        }
    }
}