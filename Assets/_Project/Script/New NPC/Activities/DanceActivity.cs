using Data;
using PropBehaviours;
using UnityEngine;

namespace New_NPC.Activities
{
    public class DanceActivity : IActivity
    {
        public bool IsEnded { get; private set; }
        
        private DancableTile _dancableTile;
        private DanceState _danceState = DanceState.None;
        private float timer;

        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            var dancableTiles = and.GetAvaliablePropsByType<DancableTile>(ePlacementLayer.BaseSurface);
            
            if (dancableTiles.Count == 0)
            {
                IsEnded = true;
                return;
            }

            _dancableTile = dancableTiles[Random.Range(0, dancableTiles.Count)];

            if (_dancableTile == null || _dancableTile.IsOccupied)
            {
                IsEnded = true;
                return;
            }

            if (DiscoData.Instance.placementDataHandler.ContainsKey(_dancableTile.CellPosition, ePlacementLayer.FloorProp))
            {
                IsEnded = true;
                return;
            }

            bool foundPath = and.Npc.pathFinder.TryGoTargetDestination(_dancableTile.CellPosition);
            if (!foundPath)
            {
                IsEnded = true;
                return;
            }
            
            and.Npc.SetAnimation(eNpcAnimation.Walk);
            _dancableTile.GetItOccupied(and.Npc);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            switch (_danceState)
            {
                case DanceState.None:
                    if (and.Npc.pathFinder.hasReachedDestination)
                    {
                        and.Npc.SetAnimation(eNpcAnimation.Dance);
                        and.Npc.GetAnimationControl.SetRootMotion(true);
                        _danceState = DanceState.Dancing;
                    }
                    break;
                case DanceState.Dancing:
                    timer += Time.deltaTime;
                    if (timer > and.Npc.GetAnimationControl.GetCurrentAnimationDuration())
                    {
                        timer = 0;
                        IsEnded = true;
                    }
                    break;
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.SetAnimation(eNpcAnimation.Idle);
            if (_dancableTile != null) _dancableTile.IsOccupied = false;
            and.Npc.GetAnimationControl.SetRootMotion(false);
        }

        private enum DanceState
        {
            None,
            Dancing
        }
    }
}