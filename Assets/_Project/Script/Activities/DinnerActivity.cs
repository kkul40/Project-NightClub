using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Activities
{
    [Serializable]
    public class DinnerActivity : Activity
    {
        private enum DinnerState
        {
            None,
            Sitting,
            Eating,
            Drinking,
        }
    
        private Table tableProp;
        private Chair chairProp;
        private Vector3 positionBeforeSit = -Vector3.one;
        private DinnerState _dinnerState = DinnerState.None;

        private float timer = 0;
        private float waitingTime = 3;
        private float eatingTime = 3;
        private float drinkingTime = 3;

        public override bool isEnded { get; protected set; }
        public override bool isCanceled { get; protected set; }

        public override void StartActivity(NPC npc)
        {
            if (isCanceled) return;
            tableProp = GetAvaliablePropByType<Table>(npc);

            if (tableProp == null || tableProp.Chairs.Count < 1)
            {
                isCanceled = true;
                return;
            }
           
            chairProp = tableProp.Chairs[Random.Range(0, tableProp.Chairs.Count)];
        
            if (chairProp == null || chairProp.IsOccupied)
            {
                isCanceled = true;
                return;
            }
        
            npc.SetNewDestination(tableProp.GetPropPosition());
            chairProp.IsOccupied = true;
        }

        public override void UpdateActivity(NPC npc)
        {
            if (isCanceled) return;
        
            switch (_dinnerState)
            {
                case DinnerState.None:
                    var distance = Vector3.Distance(npc.transform.position, tableProp.GetPropPosition());
                    if (distance < 0.05f)
                    {
                        //Deactivate Navmesh
                        //Sit
                        positionBeforeSit = npc.transform.position;
                        npc._navMeshAgent.enabled = false;
                        npc.transform.position = chairProp.GetItOccupied(npc);
                        npc.transform.rotation = chairProp.GetPropRotation();
                        npc.ChangeState(NpcState.Sit);
                        _dinnerState = DinnerState.Sitting;
                    }
                    break;
                case DinnerState.Sitting:
                    timer += Time.deltaTime;
                    if (timer > waitingTime)
                    {
                        timer = 0;
                        _dinnerState = DinnerState.Eating;
                    }
                    // Waiting logic here
                    break;
                case DinnerState.Eating:
                    // Eating logic here
                    timer += Time.deltaTime;
                    if (timer > drinkingTime)
                    {
                        timer = 0;
                        _dinnerState = DinnerState.Drinking;
                    }
                    break;
                case DinnerState.Drinking:
                    // Drinking logic here
                    timer += Time.deltaTime;
                    if (timer > drinkingTime)
                    {
                        timer = 0;
                        isEnded = true;
                    }
                    break;
            }
        }

        public override void EndActivity(NPC npc)
        {
            if (isCanceled) return;
        
            npc.ChangeState(NpcState.Idle);
            npc._navMeshAgent.enabled = true;
            npc.transform.position = positionBeforeSit;
            chairProp.IsOccupied = false;
            isEnded = true;
        }
    }
}