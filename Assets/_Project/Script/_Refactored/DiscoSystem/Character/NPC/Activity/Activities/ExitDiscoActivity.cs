using Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DiscoSystem.Character.NPC.Activity.Activities
{
    public class ExitDiscoActivity : IActivity
    {
        enum State
        {
            WalkToExit,
            Exit,
        }
        
        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }

        private float timer = 0;
        private float delay = 1;
        private ActivityNeedsData and;

        private State _state;

        public bool CanStartActivity(ActivityNeedsData and)
        {
            this.and = and;
            return true;
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            _state = State.WalkToExit;
            and.Npc.TriggerDoor = true;
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            and.Npc.PathAgent.SetDestination(DiscoData.Instance.MapData.EnterencePosition());
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            and.Npc.PathAgent.Update(Time.deltaTime);

            switch (_state)
            {
                case State.WalkToExit:
                    if (and.Npc.PathAgent.isStopped)
                    {
                        and.Npc.PathAgent.NextPosition = DiscoData.Instance.MapData.SpawnPositon;
                        _state = State.Exit;
                    }
                    break;
                case State.Exit:
                    if (and.Npc.PathAgent.isStopped)
                    {
                        OnComplete();
                    }
                    break;
            }
        }

        private void OnComplete()
        {
            IsEnded = true;
            and.Npc.ActivityHandler.isDead = true;
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.TriggerDoor = false;
        }
    }
}