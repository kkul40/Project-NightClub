using UnityEngine;

namespace NPCBehaviour.Activities
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

        public bool ForceToQuitActivity(ActivityNeedsData and)
        {
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.PathFinder.GoTargetDestination(GetRandomDestination(and));
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
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
        }

        public Vector3 GetRandomDestination(ActivityNeedsData and)
        {
            var loopCount = 0;

            var target = and.DiscoData.MapData.GetRandomPathFinderNode();
            if (target == null)
                return and.Npc.transform.position;

            while (target.IsWall || !target.IsWalkable)
            {
                target = and.DiscoData.MapData.GetRandomPathFinderNode();

                if (Helper.IterateTo100(ref loopCount))
                {
                    Debug.Log("Target Node is Wall : " + target.IsWall);

                    IsEnded = true;
                    break;
                }
            }

            return target.WorldPos;
        }
    }
}