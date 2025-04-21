using System.Character.Bartender;
using System.Collections;
using Animancer;
using Data;
using DefaultNamespace;
using DefaultNamespace.TEST;
using DG.Tweening;
using UnityEngine;

namespace System.Character.NPC
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

    public enum eActionAnimationType
    {
        Null,
        NPC_HoldDrink,
    }
    
    public class NPCAnimationControl : IAnimationController
    {
        private NewAnimationSO _npcAnimationSo;
        public Animator animator { get; }
        public AnimancerComponent animancer { get; }
        public AnimationClip CurrentAnimation { get; private set; }

        private Transform animatorTransform;
        private AnimationClip selectedAnimationClip;

        private WeightedMaskLayers rightUpperArm;

        public NPCAnimationControl(Animator animator, AnimancerComponent animancer, NewAnimationSO npcAnimationSo, Transform animatorTransform)
        {
            this.animator = animator;
            this.animancer = animancer;
            _npcAnimationSo = npcAnimationSo;
            this.animatorTransform = animatorTransform;
            rightUpperArm = this.animatorTransform.GetComponent<WeightedMaskLayers>();
        }

        public void PlayAnimation(eAnimationType eAnimationType)
        {
            switch (eAnimationType)
            {
                case eAnimationType.NPC_Idle:
                    selectedAnimationClip = _npcAnimationSo.Idle;
                    break;
                case eAnimationType.NPC_Walk:
                    selectedAnimationClip = _npcAnimationSo.Walk;
                    break;
                case eAnimationType.NPC_Sit:
                    selectedAnimationClip = _npcAnimationSo.Sit;
                    break;
                case eAnimationType.NPC_Lean:
                    selectedAnimationClip = _npcAnimationSo.LeanOnWall[UnityEngine.Random.Range(0, _npcAnimationSo.LeanOnWall.Count)];
                    break;
                case eAnimationType.NPC_Dance:
                    selectedAnimationClip = _npcAnimationSo.Dance[UnityEngine.Random.Range(0, _npcAnimationSo.Dance.Count)];
                    break;
                default:
                    selectedAnimationClip = _npcAnimationSo.DebugAnim;
                    break;
            }

            if (CurrentAnimation == selectedAnimationClip) return;

            CurrentAnimation = selectedAnimationClip;
            animancer.Layers[0].Play(selectedAnimationClip, _npcAnimationSo.animationDuration);
            // animancer.TryPlay("S_StandToSit", 1);

            animatorTransform.localRotation = Quaternion.identity;
            animatorTransform.localPosition = Vector3.zero;
        }

        private Coroutine animeRotuine;
        
        public void PlayActionAnimation(eActionAnimationType eActionAnimationType)
        {
            switch (eActionAnimationType)
            {
                case eActionAnimationType.Null:
                    animancer.Layers[1].Stop();
                    break;
                case eActionAnimationType.NPC_HoldDrink:
                    if (animeRotuine != null)
                    {
                        DOTween.instance.StopCoroutine(animeRotuine);
                    }

                    animancer.Layers[1].Mask = _npcAnimationSo.mask;
                    animancer.Layers[1].Play(_npcAnimationSo.drinkIdle, _npcAnimationSo.animationDuration);
                    animeRotuine = DOTween.instance.StartCoroutine(CancelActionAnim());
                    break;
            }
        }

        private IEnumerator CancelActionAnim()
        {
            yield return new WaitForSeconds(2);
            animancer.Layers[1].Play(_npcAnimationSo.drinkAction, _npcAnimationSo.animationDuration);

            yield return new WaitForSeconds(2);
            animancer.Layers[1].StartFade(0, _npcAnimationSo.animationDuration);

            animeRotuine = null;
            yield return null;
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
            // animator.CrossFadeInFixedTime(selectedAnimationClip.name, _animationSo.animationDuration, 0);
            animancer.Play(selectedAnimationClip, _animationSo.animationDuration);

            animatorTransform.localRotation = Quaternion.identity;
            animatorTransform.localPosition = Vector3.zero;
        }

        public void PlayActionAnimation(eActionAnimationType eActionAnimationType)
        {
            throw new NotImplementedException();
        }
    }

    public interface IAnimationController
    {
        Animator animator { get; }

        AnimancerComponent animancer { get; }

        AnimationClip CurrentAnimation { get; }
        void PlayAnimation(eAnimationType eAnimationType);

        void PlayActionAnimation(eActionAnimationType eActionAnimationType);

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