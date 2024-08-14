using System.Data.Common;
using ScriptableObjects;
using UnityEngine;

namespace New_NPC
{
    public class NPCAnimationControl : IAnimationController
    {
        private NpcAnimationSo _npcAnimationSo;
        public Animator animator { get; }
        public AnimationClip CurrentAnimation { get; private set; }

        private Transform animatorTransform;
        private AnimationClip selectedAnimationClip;

        public NPCAnimationControl(Animator animator, NpcAnimationSo npcAnimationSo, Transform animatorTransform)
        {
            this.animator = animator;
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
                case eAnimationType.NPC_Dance:
                    selectedAnimationClip = _npcAnimationSo.Dance[Random.Range(0, _npcAnimationSo.Dance.Count)];
                    break;
                default:
                    selectedAnimationClip = _npcAnimationSo.Debug[Random.Range(0, _npcAnimationSo.Debug.Count)];
                    break;
            }

            if (CurrentAnimation == selectedAnimationClip) return;

            CurrentAnimation = selectedAnimationClip;
            animator.CrossFadeInFixedTime(selectedAnimationClip.name, _npcAnimationSo.animationDuration, 0);

            animatorTransform.localPosition = Vector3.zero;
            animatorTransform.localRotation = Quaternion.identity;
        }
    }

    public class BartenderAnimationControl : IAnimationController
    {
        private BartenderAnimationSo _animationSo;
        public Animator animator { get; private set; }
        public AnimationClip CurrentAnimation { get; private set; }

        private Transform animatorTransform;
        private AnimationClip selectedAnimationClip;

        public BartenderAnimationControl(Animator animator, BartenderAnimationSo animationSo,
            Transform animatorTransform)
        {
            this.animator = animator;
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