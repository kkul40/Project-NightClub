using ScriptableObjects;
using UnityEngine;

public class NPCAnimationControl : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private NpcAnimationSo _npcAnimationSo;
    [SerializeField] private float transitionDuration;
    private AnimationClip currentAnimation;
    private AnimationClip selectedAnimationClip;

    private Transform mTransform;

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
        _animator.CrossFadeInFixedTime(selectedAnimationClip.name, transitionDuration, 0);
        
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
    }
}