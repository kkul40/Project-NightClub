using Activities;
using UnityEngine;
using UnityEngine.AI;

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
        
        _npcAnimationControl.PlayAnimation(_state);
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
        //TODO Rotation i buradan yap
        // Quaternion rotation = Quaternion.LookRotation(targetPos - transform.position);
        // transform.DORotate(rotation.eulerAngles, 0.5f);
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
    public void SetRandomDestination()
    {
        target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count)];
        SetNewDestination(target);
    }
}


public enum NpcState
{
    Idle,
    Walk,
    Sit,
    Dance,
}