using Data;
using Disco_Building;
using UnityEngine;

namespace System.Character.NPC.Activity.Activities
{
    public class LeanOnWall : IActivity
    {
        private enum eState
        {
            Walk,
            Lean,
        }

        private eState currentState;

        private ActivityNeedsData _and;

        private PathFinderNode leanablePath = new PathFinderNode();
        private float leanTime = 10;
        private float timer = 0;

        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }
        public bool CanStartActivity(ActivityNeedsData and)
        {
            if (and.DiscoData.MapData.Path.GetAvaliableWallPaths.Count < 1) return false;

            int loopCount = 0;
            
            do
            {
                leanablePath = and.DiscoData.MapData.Path.GetAvaliableWallPaths[UnityEngine.Random.Range(0, and.DiscoData.MapData.Path.GetAvaliableWallPaths.Count)];
                if (Helper.IterateTo100(ref loopCount))
                {
                    return false;
                }

            } while (leanablePath.HasOccupied);
            
            return and.Npc.PathFinder.CheckIfPathAvaliable(leanablePath.WorldPos);
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            if (!and.DiscoData.MapData.Path.GetAvaliableWallPaths.Contains(leanablePath)) return true;
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            currentState = eState.Walk;
            leanablePath.ChangeOccupition(and.Npc, true);
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            and.Npc.PathFinder.GoTargetDestination(leanablePath.WorldPos);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            switch (currentState)
            {
                case eState.Walk:
                    if (and.Npc.PathFinder.HasReachedDestination)
                    {
                        currentState = eState.Lean;
                        and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Lean);

                        bool isLookingRight = leanablePath.GridX > leanablePath.GridY ? true : false;
                        RotationData rotationData = isLookingRight ? RotationData.Down : RotationData.Left;
                        
                        and.Npc.PathFinder.SetPositioning(newRotation:rotationData.rotation, newPosition:leanablePath.WorldPos);
                    }
                    break;
                case eState.Lean:
                    timer += Time.deltaTime;
                    if (timer > leanTime)
                    {
                        timer = 0;
                        IsEnded = true;
                    }
                    break;
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
            leanablePath.ChangeOccupition(and.Npc, false);
            IsEnded = true;
        }
    }
}