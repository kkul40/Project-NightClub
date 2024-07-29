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

            if (dancableTiles == null)
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

            if (DiscoData.Instance.placementDataHandler.ContainsKey(_dancableTile.CellPosition,
                    ePlacementLayer.FloorProp))
            {
                IsEnded = true;
                return;
            }

            var foundPath = and.Npc.PathFinder.GoTargetDestination(_dancableTile.CellPosition);
            if (!foundPath)
            {
                IsEnded = true;
                return;
            }

            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Walk);
            _dancableTile.GetItOccupied(and.Npc);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            switch (_danceState)
            {
                case DanceState.None:
                    if (and.Npc.PathFinder.HasReachedDestination)
                    {
                        and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Dance);
                        and.Npc.animationController.SetRootMotion(true);
                        _danceState = DanceState.Dancing;
                    }

                    break;
                case DanceState.Dancing:
                    timer += Time.deltaTime;
                    if (timer > and.Npc.animationController.GetCurrentAnimationDuration())
                    {
                        timer = 0;
                        IsEnded = true;
                    }

                    break;
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Idle);
            if (_dancableTile != null) _dancableTile.IsOccupied = false;
            and.Npc.animationController.SetRootMotion(false);
        }

        private enum DanceState
        {
            None,
            Dancing
        }
    }
}