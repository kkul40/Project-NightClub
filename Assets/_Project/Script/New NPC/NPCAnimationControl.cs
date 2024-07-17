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
                    selectedAnimationClip = _npcAnimationSo.Idle[Random.Range(0, _npcAnimationSo.Idle.Count)];
                    break;
                case eNpcAnimation.Walk:
                    selectedAnimationClip = _npcAnimationSo.Walk[Random.Range(0, _npcAnimationSo.Walk.Count)];
                    break;
                case eNpcAnimation.Sit:
                    selectedAnimationClip = _npcAnimationSo.Sit[Random.Range(0, _npcAnimationSo.Sit.Count)];
                    break;
                case eNpcAnimation.Dance:
                    selectedAnimationClip = _npcAnimationSo.Dance[Random.Range(0, _npcAnimationSo.Dance.Count)];
                    break;
                default:
                    selectedAnimationClip = _npcAnimationSo.Debug[Random.Range(0, _npcAnimationSo.Debug.Count)];
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