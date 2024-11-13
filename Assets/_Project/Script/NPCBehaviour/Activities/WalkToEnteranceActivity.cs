using Data;
using UnityEngine;

namespace NPCBehaviour.Activities
{
    public class WalkToEnteranceActivity : IActivity
    {
        public bool CheckForPlacementOnTop { get; } = false;
        public bool IsEnded { get; private set; }

        private float timer = 0;
        private float delay = 0.5f;

        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public bool ForceToQuitActivity(ActivityNeedsData and)
        {
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.TriggerDoor = true;
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            and.Npc.PathFinder.GoTargetDestination(DiscoData.Instance.MapData.EnterencePosition);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            if (and.Npc.PathFinder.HasReachedDestination)
            {
                and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
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
            and.Npc.TriggerDoor = false;
        }
    }
}