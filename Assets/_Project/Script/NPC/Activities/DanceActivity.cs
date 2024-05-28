using Data;
using UnityEngine;

namespace NPC.Activities
{
    public class DanceActivity : Activity
    {
        private DancableTile _dancableTile;
        private DanceState _danceState = DanceState.None;

        private float timer;
        public override bool isEnded { get; protected set; }
        public override bool isCanceled { get; protected set; }


        public override void StartActivity(NPC npc)
        {
            _dancableTile = GetAvaliablePropByType<DancableTile>(npc, ePlacementLayer.Surface);

            if (_dancableTile == null || _dancableTile.IsOccupied)
            {
                isCanceled = true;
                return;
            }

            if (GameData.Instance.placementDataHandler.ContainsKey(_dancableTile.CellPosition,
                    ePlacementLayer.Floor))
            {
                isCanceled = true;
                return;
            }

            npc.SetNewDestination(_dancableTile.GetMiddlePos);
            npc.ChangeState(eNpcAnimation.Walk);
            _dancableTile.GetItOccupied(npc);
        }

        public override void UpdateActivity(NPC npc)
        {
            if (isCanceled) return;

            switch (_danceState)
            {
                case DanceState.None:
                    var distance = Vector3.Distance(npc.transform.position, _dancableTile.GetMiddlePos);
                    if (distance < 0.05f)
                    {
                        npc._navMeshAgent.enabled = false;
                        npc.ChangeState(eNpcAnimation.Dance);
                        _danceState = DanceState.Dancing;
                        npc.GetAnimationControl().SetRootMotion(true);
                    }

                    break;
                case DanceState.Dancing:
                    timer += Time.deltaTime;
                    if (timer > npc.GetAnimationControl().GetCurrentAnimationDuration())
                    {
                        timer = 0;
                        isEnded = true;
                    }

                    break;
            }
        }

        public override void EndActivity(NPC npc)
        {
            if (isCanceled) return;

            npc.ChangeState(eNpcAnimation.Idle);
            _dancableTile.IsOccupied = false;
            npc._navMeshAgent.enabled = true;
            npc.GetAnimationControl().SetRootMotion(false);
            isEnded = true;
        }

        private enum DanceState
        {
            None,
            Dancing
        }
    }
}