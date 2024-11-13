using Data;
using PropBehaviours;
using UnityEngine;

namespace NPCBehaviour.Activities
{
    public class DanceActivity : IActivity
    {
        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }

        private DancableTile _dancableTile;
        private DanceState _danceState = DanceState.None;
        private float timer;

        private float danceDuration = 0;

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

            return and.Npc.PathFinder.CheckIfPathAvaliable(_dancableTile.WorldPos);
        }

        public bool ForceToQuitActivity(ActivityNeedsData and)
        {
            if (_dancableTile == null) return true;

            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            var foundPath = and.Npc.PathFinder.GoTargetDestination(_dancableTile.WorldPos);

            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            _dancableTile.SetOccupied(and.Npc, true);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            switch (_danceState)
            {
                case DanceState.None:
                    if (and.Npc.PathFinder.HasReachedDestination)
                    {
                        and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Dance);
                        and.Npc.AnimationController.SetRootMotion(true);

                        danceDuration = and.Npc.AnimationController.GetCurrentAnimationDuration() * 2;
                        _danceState = DanceState.Dancing;
                    }
                    break;
                case DanceState.Dancing:
                    timer += Time.deltaTime;
                    if (timer > danceDuration)
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
            if (_dancableTile != null) _dancableTile.IsOccupied = false;
            and.Npc.AnimationController.SetRootMotion(false);
        }

        private enum DanceState
        {
            None,
            Dancing
        }
    }
}