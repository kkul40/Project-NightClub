using PropBehaviours;
using UnityEngine;

namespace New_NPC.Activities
{
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

            if (_chair == null) return false;

            if (_chair.IsReservedToATable) return false;

            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            this.and = and;
            and.Npc.PathFinder.GoTargetDestination(_chair.GetFrontPosition().position,
                OnCompleteCallBack: OnReachedToChair);
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

        private void OnReachedToChair()
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