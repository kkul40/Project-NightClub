using Data;
using DiscoSystem.Building_System.GameEvents;
using ExtensionMethods;
using PropBehaviours;
using UI.Emotes;
using UnityEngine;

namespace DiscoSystem.Character.NPC.Activity.Activities
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

            if (_dancableTile.WorldPos.WorldPosToCellPos(eGridType.PlacementGrid) ==
                DiscoData.Instance.MapData.EnterencePosition().WorldPosToCellPos(eGridType.PlacementGrid))
                return false;

            return true;
            // return and.Npc.PathFinder.IsPathAvaliable(_dancableTile.WorldPos);
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            if (_dancableTile == null) return true;

            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            var foundPath = and.Npc.PathAgent.SetDestination(_dancableTile.WorldPos);

            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            _dancableTile.SetOccupied(and.Npc, true);

            // _dancableTile.OnDestroyed += () => IsEnded = true;
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            and.Npc.PathAgent.Update(Time.deltaTime);
            switch (_danceState)
            {
                case DanceState.None:
                    if (and.Npc.PathAgent.isStopped)
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
                        GameEvent.Trigger(new Event_ShowEmote(EmoteTypes.Happy, and.Npc.transform));
                        IsEnded = true;
                    }

                    break;
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
            if (_dancableTile != null)
            {
                _dancableTile.IsOccupied = false;
                // _dancableTile.OnDestroyed -= () => IsEnded = true;
            }
            and.Npc.AnimationController.SetRootMotion(false);
        }

        private enum DanceState
        {
            None,
            Dancing
        }
    }
}