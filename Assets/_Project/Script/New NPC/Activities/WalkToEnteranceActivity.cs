using Data;
using UnityEngine;

namespace New_NPC.Activities
{
    public class WalkToEnteranceActivity : IActivity
    {
        public bool IsEnded { get; private set; }

        private float timer = 0;
        private float delay = 0.5f;
        
        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Walk);
            and.Npc.PathFinder.GoTargetDestination(DiscoData.Instance.MapData.EnterencePosition, false);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            if (and.Npc.PathFinder.HasReachedDestination)
            {
                and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Idle);
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    timer = 0;
                    IsEnded = true;
                }
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
        }
    }
}