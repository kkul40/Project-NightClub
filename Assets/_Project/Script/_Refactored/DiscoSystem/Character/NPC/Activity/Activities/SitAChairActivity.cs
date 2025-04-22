using PropBehaviours;
using UnityEngine;

namespace DiscoSystem.Character.NPC.Activity.Activities
{
    public class SitAChairActivity : IActivity
    {
        private Chair _chair;

        private ActivityNeedsData and = null;

        private float timer = 0;
        private float delay = 5f;

        private eState _state = eState.Null;

        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }

        public bool CanStartActivity(ActivityNeedsData and)
        {
            _chair = and.GetAvaliablePropByType<Chair>();

            if (_chair == null) return false;

            if (_chair.IsReservedToATable) return false;

            return and.Npc.PathFinder.CheckIfPathAvaliable(_chair.GetFrontPosition().position);
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            if (_chair == null) return true;
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            this.and = and;
            and.Npc.PathFinder.GoTargetDestination(_chair.GetFrontPosition().position);
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            _chair.SetOccupied(and.Npc, true);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            switch (_state)
            {
                case eState.Null:
                    if (and.Npc.PathFinder.HasReachedDestination)
                    {
                        and.Npc.PathFinder.SetPositioning(_chair.GetFrontPosition().rotation, _chair.GetSitPosition());
                        and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Sit);
                        _state = eState.SitDown;
                    }
                    break;
                case eState.SitDown:
                    timer += Time.deltaTime;
                    if (timer > delay)
                        _state = eState.StandUp;
                    break;
                case eState.StandUp:
                    and.Npc.PathFinder.SetPositioning(newPosition: _chair.GetFrontPosition().position);
                    and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);

                    timer = 0;
                    delay = 0.5f;
                    _state = eState.End;
                    break;
                case eState.End:
                    timer += Time.deltaTime;
                    if (timer > delay)
                        IsEnded = true;
                    break;
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
            if (_chair != null)  _chair.SetOccupied(and.Npc, false);
        }

        private enum eState
        {
            Null,
            SitDown,
            StandUp,
            End,
        }
    }
}