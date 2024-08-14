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
            var dancableTiles = and.GetAvaliablePropsByType<DancableTile>();

            if (dancableTiles == null)
                return false;

            _dancableTile = dancableTiles[Random.Range(0, dancableTiles.Count)];

            if (_dancableTile == null || _dancableTile.IsOccupied)
                return false;

            if (DiscoData.Instance.placementDataHandler.ContainsKey(_dancableTile.CellPosition,
                    ePlacementLayer.FloorProp))
                return false;

            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            var foundPath = and.Npc.PathFinder.GoTargetDestination(_dancableTile.WorldPos);

            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Walk);
            _dancableTile.SetOccupied(and.Npc, true);
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