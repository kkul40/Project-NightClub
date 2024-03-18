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
    private NavMeshAgent _navMeshAgent;
    private Vector3 target;
    public float speed;

    private void Awake()
    {
        _npcAnimationControl = GetComponentInChildren<NPCAnimationControl>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(SetTarget());
    }

    private IEnumerator SetTarget()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (_navMeshAgent.remainingDistance < 0.1f)
            {
                target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count - 1)];
                _navMeshAgent.SetDestination(target);
            }
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
