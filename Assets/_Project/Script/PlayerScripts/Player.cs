using System;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerScripts
{
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
                // SetDestination(BuildingSystem.Instance.GetCellCenterWorld(BuildingSystem.Instance.GetWorldToCell(InputSystem.Instance.GetMouseMapPosition())));
            }
        }

        private void SetDestination(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
        }
    }
}