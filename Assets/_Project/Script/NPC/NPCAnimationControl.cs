using ScriptableObjects;
using UnityEngine;

public class NPCAnimationControl : MonoBehaviour
{
    [SerializeField] private NpcAnimationSo _npcAnimationSo;
    private Animator _animator;
    private AnimationClip currentAnimation;

    private Transform mTransform;
    private AnimationClip selectedAnimationClip;

    private void Awake()
    {
        // TODO Animation Ile dene
        _animator = GetComponent<Animator>();
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

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
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