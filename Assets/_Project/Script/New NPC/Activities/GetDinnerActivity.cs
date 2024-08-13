using Data;
using DG.Tweening;
using PropBehaviours;
using UnityEngine;

namespace New_NPC.Activities
{
    public class GetDinnerActivity : IActivity
    {
         private DinnerState _dinnerState = DinnerState.None;
         private Chair chairProp;
         private float drinkingTime = 3;
         private float eatingTime = 3;

         private Table tableProp;

         private float timer;
         private float tweenDuration = 0.5f;
         private float waitingTime = 3;
         
         public bool IsEnded { get; private set; }
         
         public bool CanStartActivity(ActivityNeedsData and)
         {
             tableProp = and.GetAvaliablePropByType<Table>();

             if (tableProp == null || tableProp.Chairs.Count < 1)
             {
                 return false;
             }

             chairProp = tableProp.Chairs[Random.Range(0, tableProp.Chairs.Count)];

             if (chairProp == null || chairProp.IsOccupied)
             {
                 return false;
             }

             return true;
         }

         public void OnActivityStart(ActivityNeedsData and)
         {
             and.Npc.PathFinder.GoTargetDestination(chairProp.GetFrontPosition().position);
             and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Walk);
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
                         and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Sit);
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
                         timer = 0;
                         IsEnded = true;
                     }
                    break;
             }
         }

         public void OnActivityEnd(ActivityNeedsData and)
         {
             and.Npc.PathFinder.SetPositioning(newPosition: chairProp.GetFrontPosition().position);
             chairProp.SetOccupied(and.Npc, false);
             and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Idle);
             IsEnded = true;
         }

         private enum DinnerState
         {
             None,
             Sitting,
             Eating,
             Drinking
         }
    }
    
    
    public class SitAChairActivity : IActivity
    {
        private Chair _chair;

        private ActivityNeedsData and = null;

        private float timer = 0;
        private float delay = 5f;
        private bool startTimer = false;
        
        public bool IsEnded { get; private set; }
        public bool CanStartActivity(ActivityNeedsData and)
        {
            _chair = and.GetAvaliablePropByType<Chair>();

            if (_chair == null || !_chair.IsOccupied) return false;
            
            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            this.and = and;
            and.Npc.PathFinder.GoTargetDestination(_chair.GetFrontPosition().position, OnCompleteCallBack: ReachTheChair);
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Walk);
            _chair.SetOccupied(and.Npc, true);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            if (startTimer)
            {
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    timer = 0;
                    IsEnded = true;
                }
            }
        }

        private void ReachTheChair()
        {
            and.Npc.PathFinder.SetPositioning(_chair.GetFrontPosition().rotation, _chair.GetSitPosition());
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Sit);
            startTimer = true;
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            // TODO Kalkarken Bi yamukluk yasaniyor, bir ara duzelt
            _chair.SetOccupied(and.Npc, false);
            and.Npc.PathFinder.SetPositioning(newPosition: _chair.GetFrontPosition().position);
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Idle);
        }
    }
}