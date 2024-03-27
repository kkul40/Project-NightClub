using System;
using BuildingSystemFolder;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (InputSystem.Instance.RightClickOnWorld)
        {
            SetDestination(BuildingSystem.Instance.GetGrid().GetCellCenterWorld(BuildingSystem.Instance.GetGrid().WorldToCell(InputSystem.Instance.GetMouseMapPosition())));
        }
    }

    private void SetDestination(Vector3 destination)
    {
        _navMeshAgent.SetDestination(destination);
    }
}