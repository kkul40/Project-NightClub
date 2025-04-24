using Data;
using UnityEngine;

namespace DiscoSystem.Character.NPC.Activity.Activities
{
    public class ExitDiscoActivity : IActivity
    {
        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }

        private float timer = 0;
        private float delay = 1;
        private ActivityNeedsData and;

        public bool CanStartActivity(ActivityNeedsData and)
        {
            this.and = and;
            return true;
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.TriggerDoor = true;
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            and.Npc.PathFinder.GoToDestination(DiscoData.Instance.MapData.EnterencePosition());
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            if (and.Npc.PathFinder.HasReachedDestination)
                and.Npc.PathFinder.GoToDestination(DiscoData.Instance.MapData.SpawnPositon, OnComplete);
        }

        private void OnComplete()
        {
            Object.DestroyImmediate(and.Npc.gameObject);
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.TriggerDoor = false;
        }
    }
}