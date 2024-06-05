using System;
using Data;
using DG.Tweening;
using New_NPC;
using PropBehaviours;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Activities
{
    [Serializable]
    public class DinnerActivity : Activity
    {
        private DinnerState _dinnerState = DinnerState.None;
        private Chair chairProp;
        private float drinkingTime = 3;
        private float eatingTime = 3;

        private Table tableProp;

        private float timer;
        private float tweenDuration = 0.5f;
        private float waitingTime = 3;

        public override bool isEnded { get; protected set; }
        public override bool isCanceled { get; protected set; }

        public override void StartActivity(New_NPC.NPC npc)
        {
            if (isCanceled) return;
            tableProp = GetAvaliablePropByType<Table>(npc, ePlacementLayer.Floor);

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

            npc.SetNewDestination(chairProp.GetFrontPosition());
            chairProp.IsOccupied = true;
        }

        public override void UpdateActivity(New_NPC.NPC npc)
        {
            if (isCanceled) return;

            switch (_dinnerState)
            {
                case DinnerState.None:
                    var distance = Vector3.Distance(npc.transform.position, chairProp.GetFrontPosition());
                    if (distance < 0.05f)
                    {
                        //Deactivate Navmesh
                        //Sit
                        npc._navMeshAgent.enabled = false;
                        chairProp.GetItOccupied(npc);
                        npc.transform.DOMove(chairProp.GetSitPosition(), tweenDuration);
                        // npc.transform.rotation = chairProp.GetPropRotation();
                        npc.transform.DORotate(chairProp.GetPropRotation().eulerAngles, tweenDuration);
                        npc.SetAnimation(eNpcAnimation.Sit);
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

        public override void EndActivity(New_NPC.NPC npc)
        {
            if (isCanceled) return;

            npc._navMeshAgent.enabled = true;
            npc.transform.DOMove(chairProp.GetFrontPosition(), tweenDuration);
            chairProp.IsOccupied = false;
            npc.SetAnimation(eNpcAnimation.Idle);
            isEnded = true;
        }

        private enum DinnerState
        {
            None,
            Sitting,
            Eating,
            Drinking
        }
    }
}