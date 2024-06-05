using Data;
using PropBehaviours;
using UnityEngine;

namespace New_NPC.Activities
{
    public class DanceActivity : IActivity
    {
        private DancableTile _dancableTile;
        private DanceState _danceState;
        private float timer;

        public bool IsEnded { get; private set; }
        
        public void StartActivity(ActivityNeedsData and)
        {
            _dancableTile = and.GetAvaliablePropByType<DancableTile>(ePlacementLayer.Surface);

            if (_dancableTile == null || _dancableTile.IsOccupied)
            {
                IsEnded = true;
                return;
            }

            if (DiscoData.Instance.placementDataHandler.ContainsKey(_dancableTile.CellPosition,
                    ePlacementLayer.Floor))
            {
                IsEnded = true;
                return;
            }
            
            and.Npc.SetNewDestination(_dancableTile.GetMiddlePos);
            and.Npc.SetAnimation(eNpcAnimation.Walk);
            _dancableTile.GetItOccupied(and.Npc);
        }

        public void UpdateActivity(ActivityNeedsData and)
        {
            switch (_danceState)
            {
                case DanceState.None:
                    var distance = Vector3.Distance(and.Npc.transform.position, _dancableTile.GetMiddlePos);
                    if (distance < 0.05f)
                    {
                        and.Npc._navMeshAgent.enabled = false;
                        and.Npc.SetAnimation(eNpcAnimation.Dance);
                        _danceState = DanceState.Dancing;
                        and.Npc.GetAnimationControl.SetRootMotion(true);
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
            if(_dancableTile != null) _dancableTile.IsOccupied = false;
            and.Npc._navMeshAgent.enabled = true;
            and.Npc.GetAnimationControl.SetRootMotion(false);
        }
        
        private enum DanceState
        {
            None,
            Dancing
        }
    }
}