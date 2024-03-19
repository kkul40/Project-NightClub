using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[SelectionBase]
public class NPC : MonoBehaviour
{
    [SerializeField] private NpcState _state;
    private NPCAnimationControl _npcAnimationControl;
    public float speed;

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
        _state = NpcState.Idle;
    }

    private void Update()
    {
        UpdateActivity();
        
        Debug.Log(_state.ToString());
        
        _npcAnimationControl.PlayAnimation(_state);
        // Test
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

        if (!currentActivity.isEnded)
        {
            currentActivity.UpdateActivity(this);
        }
    }

    public void SetNewTarget(Vector3 targetPos)
    {
        _navMeshAgent.SetDestination(targetPos);
        _state = NpcState.Walk;
    }

    public void ChangeState(NpcState newState)
    {
        _state = newState;
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
    
    [ContextMenu("Random Target")]
    public void SetRandomTarget()
    {
        target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count - 1)];
        _navMeshAgent.SetDestination(target);
    }

    private IEnumerator StartRandomWalkingCo()
    {
        yield return new WaitForSeconds(0.1f);
        SetRandomTarget();
        while (true)
        {
            Debug.Log(_navMeshAgent.hasPath);
            if (!_navMeshAgent.hasPath)
            {
                yield return new WaitForSeconds(2);
                SetRandomTarget();
            }
            yield return new WaitForFixedUpdate();
        }
    }
}


public enum NpcState
{
    Idle,
    Walk,
    Sit,
    Dance,
}
