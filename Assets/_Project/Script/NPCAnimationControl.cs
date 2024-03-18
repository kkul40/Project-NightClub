using System;
using _Project.Script.NewSystem.ScriptableObjects;
using UnityEngine;

public class NPCAnimationControl : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private NpcAnimationSo _npcAnimationSo;
    [SerializeField] private float transitionDuration;
    private AnimationClip currentAnimation;
    private AnimationClip selectedAnimationClip;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(NpcState npcState)
    {
        switch (npcState)
        {
            case NpcState.Idle:
                selectedAnimationClip = _npcAnimationSo.Idle;
                break;
            case NpcState.Walk:
                selectedAnimationClip = _npcAnimationSo.Walk;
                break;
            case NpcState.Sit:
                selectedAnimationClip = _npcAnimationSo.Sit;
                break;
            case NpcState.Dance:
                selectedAnimationClip = _npcAnimationSo.Dance;
                break;
            default:
                selectedAnimationClip = _npcAnimationSo.Debug;
                break;
        }

        if (currentAnimation == selectedAnimationClip) return;

        currentAnimation = selectedAnimationClip;
        _animator.CrossFadeInFixedTime(selectedAnimationClip.name, transitionDuration, 0);
    }
}
