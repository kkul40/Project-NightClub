using Animancer;
using DiscoSystem.Character.NPC;
using UnityEngine;

namespace TEST
{
    public class AnimationTestNPC : MonoBehaviour
    {
        public IAnimationController AnimationController { get; private set; }
        
        public void Initialize(NewAnimationSO npcAnimationSo, Animator animator, AnimancerComponent animancerComponent, Transform armatureTransform)
        {
            AnimationController = new NPCAnimationControl(animator, animancerComponent, npcAnimationSo, armatureTransform);
        }
    }
}