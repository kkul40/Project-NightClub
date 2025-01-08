using Animancer;
using ScriptableObjects;
using UnityEngine;

namespace NPCBehaviour
{
    public enum eAnimationType
    {
        NPC_Idle,
        NPC_Walk,
        NPC_Sit,
        NPC_Dance,
        NPC_Lean,
        Bartender_Idle,
        Bartender_Walk,
        Bartender_PrepareDrink,
        Bartender_CleanUpTable
    }
    
    public class NPCAnimationControl : IAnimationController
    {
        private NpcAnimationSo _npcAnimationSo;
        public Animator animator { get; }
        public AnimancerComponent animancer { get; }
        public AnimationClip CurrentAnimation { get; private set; }

        private Transform animatorTransform;
        private AnimationClip selectedAnimationClip;

        public NPCAnimationControl(Animator animator, AnimancerComponent animancer, NpcAnimationSo npcAnimationSo, Transform animatorTransform)
        {
            this.animator = animator;
            this.animancer = animancer;
            _npcAnimationSo = npcAnimationSo;
            this.animatorTransform = animatorTransform;
        }

        public void PlayAnimation(eAnimationType eAnimationType)
        {
            switch (eAnimationType)
            {
                case eAnimationType.NPC_Idle:
                    selectedAnimationClip = _npcAnimationSo.Idle[Random.Range(0, _npcAnimationSo.Idle.Count)];
                    break;
                case eAnimationType.NPC_Walk:
                    selectedAnimationClip = _npcAnimationSo.Walk[Random.Range(0, _npcAnimationSo.Walk.Count)];
                    break;
                case eAnimationType.NPC_Sit:
                    selectedAnimationClip = _npcAnimationSo.Sit[Random.Range(0, _npcAnimationSo.Sit.Count)];
                    break;
                case eAnimationType.NPC_Lean:
                    selectedAnimationClip = _npcAnimationSo.LeanOnWall[Random.Range(0, _npcAnimationSo.LeanOnWall.Count)];
                    break;
                case eAnimationType.NPC_Dance:
                    selectedAnimationClip = _npcAnimationSo.Dance[Random.Range(0, _npcAnimationSo.Dance.Count)];
                    break;
                default:
                    selectedAnimationClip = _npcAnimationSo.Debug[Random.Range(0, _npcAnimationSo.Debug.Count)];
                    break;
            }

            if (CurrentAnimation == selectedAnimationClip) return;

            CurrentAnimation = selectedAnimationClip;
            // animator.CrossFadeInFixedTime(selectedAnimationClip.name, _npcAnimationSo.animationDuration, 0);
            animancer.Play(selectedAnimationClip, _npcAnimationSo.animationDuration);

            animatorTransform.localRotation = Quaternion.identity;
            animatorTransform.localPosition = Vector3.zero;
        }
    }

    public class BartenderAnimationControl : IAnimationController
    {
        private BartenderAnimationSo _animationSo;
        public Animator animator { get; private set; }
        public AnimancerComponent animancer { get; }
        public AnimationClip CurrentAnimation { get; private set; }

        private Transform animatorTransform;
        private AnimationClip selectedAnimationClip;

        public BartenderAnimationControl(Animator animator, AnimancerComponent animancer, BartenderAnimationSo animationSo,
            Transform animatorTransform)
        {
            this.animator = animator;
            this.animancer = animancer;
            _animationSo = animationSo;
            this.animatorTransform = animatorTransform;

            PlayAnimation(eAnimationType.Bartender_Idle);
        }

        public void PlayAnimation(eAnimationType eAnimationType)
        {
            switch (eAnimationType)
            {
                case eAnimationType.Bartender_Idle:
                    selectedAnimationClip = _animationSo.Idle;
                    break;
                case eAnimationType.Bartender_Walk:
                    selectedAnimationClip = _animationSo.Walk;
                    break;
                case eAnimationType.Bartender_PrepareDrink:
                    selectedAnimationClip = _animationSo.PrepareDrink;
                    break;
                case eAnimationType.Bartender_CleanUpTable:
                    selectedAnimationClip = _animationSo.CleanUpTable;
                    break;
                default:
                    selectedAnimationClip = _animationSo.Debug;
                    break;
            }

            if (CurrentAnimation == selectedAnimationClip) return;

            CurrentAnimation = selectedAnimationClip;
            animator.CrossFadeInFixedTime(selectedAnimationClip.name, _animationSo.animationDuration, 0);

            animatorTransform.localRotation = Quaternion.identity;
            animatorTransform.localPosition = Vector3.zero;
        }
    }

    public interface IAnimationController
    {
        Animator animator { get; }

        AnimancerComponent animancer { get; }

        AnimationClip CurrentAnimation { get; }
        void PlayAnimation(eAnimationType eAnimationType);

        void SetRootMotion(bool applyRootMotion)
        {
            animator.applyRootMotion = applyRootMotion;
        }

        float GetCurrentAnimationDuration()
        {
            return CurrentAnimation.length;
        }
    }
}