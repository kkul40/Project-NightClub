using ScriptableObjects;
using UnityEngine;

namespace New_NPC
{
    public class NPCAnimationControl
    {
        private NpcAnimationSo _npcAnimationSo;
        private Animator _animator;
        private AnimationClip currentAnimation;

        private Transform animatorTransform;
        private AnimationClip selectedAnimationClip;

        public NPCAnimationControl(Animator animator, NpcAnimationSo npcAnimationSo, Transform animatorTransform)
        {
            _animator = animator;
            _npcAnimationSo = npcAnimationSo;
            this.animatorTransform = animatorTransform;
        }

        public void PlayAnimation(eNpcAnimation eNpcAnimation)
        {
            switch (eNpcAnimation)
            {
                case eNpcAnimation.Idle:
                    selectedAnimationClip = _npcAnimationSo.Idle;
                    break;
                case eNpcAnimation.Walk:
                    selectedAnimationClip = _npcAnimationSo.Walk;
                    break;
                case eNpcAnimation.Sit:
                    selectedAnimationClip = _npcAnimationSo.Sit;
                    break;
                case eNpcAnimation.Dance:
                    selectedAnimationClip = _npcAnimationSo.Dance;
                    break;
                default:
                    selectedAnimationClip = _npcAnimationSo.Debug;
                    break;
            }

            if (currentAnimation == selectedAnimationClip) return;

            currentAnimation = selectedAnimationClip;
            _animator.CrossFadeInFixedTime(selectedAnimationClip.name, _npcAnimationSo.animationDuration, 0);
            
            animatorTransform.localPosition = Vector3.zero;
            animatorTransform.localRotation = Quaternion.identity;
        }

        public void SetRootMotion(bool appyRootMotion)
        {
            _animator.applyRootMotion = appyRootMotion;
        }

        public float GetCurrentAnimationDuration()
        {
            return currentAnimation.length;
        }
    }
}