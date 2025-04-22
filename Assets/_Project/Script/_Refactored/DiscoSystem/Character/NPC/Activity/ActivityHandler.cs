using System.Collections.Generic;
using Data;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Character.NPC.Activity.Activities;
using ExtensionMethods;
using UI.Emotes;
using UnityEngine;

namespace DiscoSystem.Character.NPC.Activity
{
    public class ActivityHandler
    {
        private ActivityNeedsData _activityNeedsData;
        private IActivity _lastActivity;
        private IActivity _currentActivity;
        private ActivityGiver _activityGiver;

        public bool hasActivity => _currentActivity != null;

        public IActivity GetCurrentActivity => _currentActivity;

        public ActivityHandler(NPC npc, DiscoData discoData)
        {
            _activityGiver = new ActivityGiver();
            _activityNeedsData = new ActivityNeedsData(npc, discoData);

            _currentActivity = new WalkToEnteranceActivity();
            _lastActivity = _currentActivity;
            _currentActivity.OnActivityStart(_activityNeedsData);
        }

        public void StartNewActivity(IActivity activity)
        {
            if (!activity.CanStartActivity(_activityNeedsData)) return;

            if(hasActivity)
                _currentActivity.OnActivityEnd(_activityNeedsData);
            
            _currentActivity = activity;
            _lastActivity = _currentActivity;
            _currentActivity.OnActivityStart(_activityNeedsData);
        }

        public void UpdateActivity()
        {
            if (!hasActivity) return;

            if (_currentActivity.IsEnded)
            {
                StartNewActivity(GetRandomActivity());
                return;
            }
            else if (_currentActivity.OnActivityErrorHandler(_activityNeedsData))
            {
                GameEvent.Trigger(new Event_ShowEmote(EmoteTypes.Angry, _activityNeedsData.Npc.transform));
                
                _currentActivity.OnActivityEnd(_activityNeedsData);
                StartNewActivity(new ExitDiscoActivity());
                return;
            }

            _currentActivity.OnActivityUpdate(_activityNeedsData);
        }

        private IActivity GetRandomActivity()
        {
            var randomActivity = _activityGiver.GetRandomActivity();

            var iterate = 0;
            while (_lastActivity.GetType() == randomActivity.GetType() || !randomActivity.CanStartActivity(_activityNeedsData))
            {
                randomActivity = _activityGiver.GetRandomActivity();

                if (Helper.IterateTo100(ref iterate))
                {
                    randomActivity = new WalkRandomActivity();
                    break;
                }
            }

            return randomActivity;
        }
        
        public void HasPlacementOnTop(List<Vector3Int> keys)
        {
            if (!hasActivity || !_currentActivity.CheckForPlacementOnTop) return;
            
            foreach (var key in keys)
            {
                if (!_activityNeedsData.Npc.PathFinder.HasReachedDestination)
                {
                    // Is Placement on The Destination
                    if (key == _activityNeedsData.Npc.PathFinder.TargetPosition.WorldPosToCellPos(eGridType.PlacementGrid))
                    {
                        // UIEmoteManager.Instance.ShowEmote(EmoteTypes.Exclamation, _activityNeedsData.Npc);
                        StartNewActivity(GetRandomActivity());
                        return;
                    }
                }
                else
                {
                    // Is Placement on your way
                    foreach (var path in _activityNeedsData.Npc.PathFinder.FoundPath) 
                    {
                        if (path.WorldPosToCellPos(eGridType.PlacementGrid) == key)
                        {
                            if (_activityNeedsData.Npc.PathFinder.GoTargetDestination(_activityNeedsData.Npc.PathFinder.TargetPosition))
                                return;
                        
                            GameEvent.Trigger(new Event_ShowEmote(EmoteTypes.Exclamation, _activityNeedsData.Npc.transform));
                            StartNewActivity(GetRandomActivity());
                            return;
                        }
                    }
                }
               
                // Is Placement On Top Of You
                if (key == _activityNeedsData.Npc.transform.position.WorldPosToCellPos(eGridType.PlacementGrid))
                {
                    GameEvent.Trigger(new Event_ShowEmote(EmoteTypes.Exclamation, _activityNeedsData.Npc.transform));
                    StartNewActivity(GetRandomActivity());
                    return;
                }
            }
        }
        
        /// <summary>
        /// Use it OnDestroy To Avoid Event Problems!
        /// </summary>
        public void ForceToEndActivity()
        {
            if (hasActivity)
                _currentActivity.OnActivityEnd(_activityNeedsData);

            // PlacementDataHandler.OnPlacedPositions -= HasPlacementOnTop;
        }
    }
}