using UnityEngine;

namespace DiscoSystem.Character.NPC.Activity.Activities
{
    public class WalkRandomActivity : IActivity
    {
        private readonly float delay = 2;
        private float timer;

        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }

        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            and.Npc.PathAgent.SetRandomDestination();
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            and.Npc.PathAgent.Update(Time.deltaTime);
            if (and.Npc.PathAgent.isStopped)
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
        }

        
        // private bool CheckTargetDestinationForHeight(ActivityNeedsData and, PathFinderNode target)
        // {
        //     return and.DiscoData.placementDataHandler.ContainsKeyOnWall(
        //         target.WorldPos.WorldPosToCellPos(eGridType.PlacementGrid), 2);
        // }
        //
        // private bool CheckTargetDestinationForDanceFloor(ActivityNeedsData and, PathFinderNode target)
        // {
        //     return and.DiscoData.placementDataHandler.ContainsKey(
        //         target.WorldPos.WorldPosToCellPos(eGridType.PlacementGrid), ePlacementLayer.BaseSurface);
        // }
    }
}