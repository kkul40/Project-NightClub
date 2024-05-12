using System;
using Activities;
using BuildingSystemFolder;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[SelectionBase]
public class NPC : MonoBehaviour, ICursorInteraction
{
    [SerializeField] private eNpcAnimation _animationState;
    private NPCAnimationControl _npcAnimationControl;

    public NavMeshAgent _navMeshAgent { get ; private set; }
    private Vector3 target;

    private Activity currentActivity;

    private void Awake()
    {
        _npcAnimationControl = GetComponentInChildren<NPCAnimationControl>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _animationState = eNpcAnimation.Idle;
    }

    private void Update()
    {
        UpdateActivity();
        _npcAnimationControl.PlayAnimation(_animationState);
    }

    private void UpdateActivity()
    {
        if (currentActivity == null)
        {
            //TODO SImdilik Random
            ChangeActivitiy(ActivitySystem.Instance.GetRandomActivity());
            return;
        }

        if (currentActivity.isCanceled || currentActivity.isEnded)
        {
            ChangeActivitiy(ActivitySystem.Instance.GetRandomActivity());
            return;
        }
        else
        {
            Debug.LogWarning(currentActivity);
            currentActivity.UpdateActivity(this);
        }
    }

    public void SetNewDestination(Vector3 targetPos)
    {
        _navMeshAgent.SetDestination(targetPos);
    }

    public void SetRotation(Quaternion targetRotation)
    {
        transform.DORotate(targetRotation.eulerAngles, 0.5f);
    }

    public void ChangeState(eNpcAnimation newAnimation)
    {
        _animationState = newAnimation;
    }

    public void ChangeActivitiy(Activity newActivity)
    {
        if (newActivity == null) return;
        
        if (currentActivity == null)
        {
            currentActivity = newActivity;
            currentActivity.StartActivity(this);
            return;
        }
        
        currentActivity.EndActivity(this);
        currentActivity = newActivity;
        currentActivity.StartActivity(this);
    }

    public NPCAnimationControl GetAnimationControl() => _npcAnimationControl;
    
    public void OnFocus()
    {
    }

    public void OnOutFocus()
    {
    }

    public void OnClick()
    {
    }
}


public enum eNpcAnimation
{
    Idle,
    Walk,
    Sit,
    Dance,
}