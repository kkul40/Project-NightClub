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
    
    private NavMeshAgent _navMeshAgent;
    private Vector3 target;

    private void Awake()
    {
        _npcAnimationControl = GetComponentInChildren<NPCAnimationControl>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(StartWalkingCoTest());
    }
    private void Update()
    {
        if (!_navMeshAgent.hasPath)
        {
            _state = NpcState.Idle;
        }
        else
        {
            _state = NpcState.Walk;
        }
        _npcAnimationControl.PlayAnimation(_state);
        // transform.position += transform.forward * Time.deltaTime * speed;
    }
    
    

    [ContextMenu("Random Target")]
    public void SetRandomTarget()
    {
        target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count - 1)];
        _navMeshAgent.SetDestination(target);
    }

    private IEnumerator StartWalkingCoTest()
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
        
        yield break;
    }
    
    
   
}

public enum NpcState
{
    Idle,
    Walk,
    Sit,
    Dance,
}
