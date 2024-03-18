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
        StartCoroutine(test());
    }


    [ContextMenu("Random Target")]
    public void SetRandomTarget()
    {
        target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count - 1)];
        _navMeshAgent.SetDestination(target);
        _state = NpcState.Walk;
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(0.1f);
        SetRandomTarget();
        while (true)
        {
            Debug.Log(_navMeshAgent.hasPath);
            if (!_navMeshAgent.hasPath)
            {
                _state = NpcState.Idle;
                yield return new WaitForSeconds(2);
                SetRandomTarget();
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    
    private void Update()
    {
        
        _npcAnimationControl.PlayAnimation(_state);
        // transform.position += transform.forward * Time.deltaTime * speed;
    }
}

public enum NpcState
{
    Idle,
    Walk,
    Sit,
    Dance,
}
