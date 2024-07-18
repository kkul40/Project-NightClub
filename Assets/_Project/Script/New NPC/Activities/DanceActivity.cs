using Data;
using PropBehaviours;
using UnityEngine;

namespace New_NPC.Activities
{
    public class DanceActivity : IActivity
    {
        private DancableTile _dancableTile;
        private DanceState _danceState = DanceState.None;
        private float timer;

        public bool IsEnded { get; private set; }

        public void StartActivity(ActivityNeedsData and)
        {
            _dancableTile = and.GetAvaliablePropByType<DancableTile>(ePlacementLayer.BaseSurface);

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

            and.Npc.SetAnimation(eNpcAnimation.Walk);
            _dancableTile.GetItOccupied(and.Npc);
            and.Npc.pathFinder.GoToDestination(_dancableTile.CellPosition);
        }

        public void UpdateActivity(ActivityNeedsData and)
        {
            switch (_danceState)
            {
                case DanceState.None:
                    if (and.Npc.pathFinder.hasReachedDestination)
                    {
                        Debug.Log("Reach the target dance");
                        // and.Npc._navMeshAgent.enabled = false;
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

        public void EndActivity(ActivityNeedsData and)
        {
            and.Npc.SetAnimation(eNpcAnimation.Idle);
            if (_dancableTile != null) _dancableTile.IsOccupied = false;
            // and.Npc._navMeshAgent.enabled = true;
            and.Npc.GetAnimationControl.SetRootMotion(false);
        }

        private enum DanceState
        {
            None,
            Dancing
        }
    }
}