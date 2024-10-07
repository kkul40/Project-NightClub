using System;
using System.Collections.Generic;
using BuildingSystem;
using Data;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC_Stuff.Activities
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
        
        public bool IsEnded { get; private set; }
        public bool CanStartActivity(ActivityNeedsData and)
        {
            if (and.DiscoData.MapData.GetAvaliablePathsNearWall.Count < 1) return false;

            int loopCount = 0;
            
            do
            {
                leanablePath = and.DiscoData.MapData.GetAvaliablePathsNearWall[Random.Range(0, and.DiscoData.MapData.GetAvaliablePathsNearWall.Count)];
                if (Helper.IterateTo100(ref loopCount))
                {
                    return false;
                }

            } while (leanablePath.HasOccupied);

            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            _and = and;
            currentState = eState.Walk;
            leanablePath.ChangeOccupition(and.Npc, true);
            and.Npc.PathFinder.GoTargetDestination(leanablePath.WorldPos);
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            
            PlacementDataHandler.OnPlacedPositions += HasPlacedOnTop;
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
            PlacementDataHandler.OnPlacedPositions -= HasPlacedOnTop;
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
            leanablePath.ChangeOccupition(and.Npc, false);
        }

        private void HasPlacedOnTop(List<Vector3Int> keys)
        {
            foreach (var key in keys)
            {
                if (currentState == eState.Lean) // Is Placement On Top of you
                {
                    if (key == _and.Npc.PathFinder.mTransform.position.WorldPosToCellPos(eGridType.PlacementGrid))
                    {
                        IsEnded = true;
                        return;
                    }
                }
                else
                {
                    IsEnded = true;
                    return;
                }
                
                if (key == leanablePath.WorldPos.WorldPosToCellPos(eGridType.PlacementGrid)) // Is Placement on your destination
                {
                    IsEnded = true;
                    return;
                }
                
                foreach (var path in _and.Npc.PathFinder.FoundPath) // Is Placement on your way
                {
                    if (path.WorldPosToCellPos(eGridType.PlacementGrid) == key)
                    {
                        _and.Npc.PathFinder.GoTargetDestination(leanablePath.WorldPos);
                        return;
                    }
                }
            }
        }
    }
}