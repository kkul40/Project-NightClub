using Data;
using DG.Tweening;
using PropBehaviours;
using UnityEngine;

namespace NPC_Stuff.Activities
{
    public class DinnerActivity : IActivity
    {
        private DinnerState _dinnerState = DinnerState.None;
        private Chair chairProp;
        private float drinkingTime = 3;
        private float eatingTime = 3;

        private Table tableProp;

        private float timer;
        private float tweenDuration = 0.5f;
        private float waitingTime = 3;

        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }

        public bool CanStartActivity(ActivityNeedsData and)
        {
            tableProp = and.GetAvaliablePropByType<Table>();

            if (tableProp == null || tableProp.Chairs.Count < 1) return false;

            chairProp = tableProp.Chairs[Random.Range(0, tableProp.Chairs.Count)];

            if (chairProp == null || chairProp.IsOccupied) return false;

            return and.Npc.PathFinder.CheckIfPathAvaliable(chairProp.GetFrontPosition().position);
        }

        public bool ForceToQuitActivity(ActivityNeedsData and)
        {
            if (chairProp == null || tableProp == null) return true;
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.PathFinder.GoTargetDestination(chairProp.GetFrontPosition().position);
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            chairProp.SetOccupied(and.Npc, true);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            switch (_dinnerState)
            {
                case DinnerState.None:
                    if (and.Npc.PathFinder.HasReachedDestination)
                    {
                        and.Npc.PathFinder.SetPositioning(chairProp.GetFrontPosition().rotation, chairProp.GetSitPosition());
                        and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Sit);
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
                    timer += Time.deltaTime;
                    if (timer > drinkingTime)
                    {
                        _dinnerState = DinnerState.StandUp;
                    }
                    break;
                case DinnerState.StandUp:
                    and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
                    and.Npc.PathFinder.SetPositioning(newPosition: chairProp.GetFrontPosition().position);

                    timer = 0;
                    _dinnerState = DinnerState.End;
                    break;
                case DinnerState.End:
                    timer += Time.deltaTime;
                    if (timer > 0.5f)
                    {
                        IsEnded = true;
                    }
                    break;
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            if(chairProp != null) chairProp.SetOccupied(and.Npc, false);
        }

        private enum DinnerState
        {
            None,
            Sitting,
            Eating,
            Drinking,
            StandUp,
            End,
        }
    }
}