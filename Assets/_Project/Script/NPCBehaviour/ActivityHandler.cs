using System;
using System.Collections.Generic;
using Data;
using ExtensionMethods;
using NPCBehaviour.Activities;
using UI.Emotes;
using UnityEngine;

namespace NPCBehaviour
{
    public class ActivityHandler
    {
        private ActivityNeedsData _activityNeedsData;
        private IActivity _lastActivity;
        private IActivity _currentActivity;

        public bool hasActivity => _currentActivity != null;

        public IActivity GetCurrentActivity => _currentActivity;

        public ActivityHandler(NPC npc)
        {
            _activityNeedsData = new ActivityNeedsData();
            _activityNeedsData.Npc = npc;
            _activityNeedsData.DiscoData = DiscoData.Instance;
            _activityNeedsData.GridHandler = GridHandler.Instance;
            
            PlacementDataHandler.OnPlacedPositions += HasPlacementOnTop;

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
            else if (_currentActivity.ForceToQuitActivity(_activityNeedsData))
            {
                UIEmoteManager.Instance.ShowEmote(EmoteTypes.Angry, _activityNeedsData.Npc);
                
                _currentActivity.OnActivityEnd(_activityNeedsData);
                StartNewActivity(new ExitDiscoActivity());
                return;
            }

            _currentActivity.OnActivityUpdate(_activityNeedsData);
        }

        private IActivity GetRandomActivity()
        {
            var randomActivity = ActivitySystem.Instance.GetRandomActivity();

            var iterate = 0;
            while (_lastActivity.GetType() == randomActivity.GetType() || !randomActivity.CanStartActivity(_activityNeedsData))
            {
                randomActivity = ActivitySystem.Instance.GetRandomActivity();

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
                // Is Placement on The Destination
                if (key == _activityNeedsData.Npc.PathFinder.TargetPosition.WorldPosToCellPos(eGridType.PlacementGrid))
                {
                    StartNewActivity(GetRandomActivity());
                    return;
                }
                
                // Is Placement on your way
                foreach (var path in _activityNeedsData.Npc.PathFinder.FoundPath) 
                {
                    if (path.WorldPosToCellPos(eGridType.PlacementGrid) == key)
                    {
                        if (_activityNeedsData.Npc.PathFinder.GoTargetDestination(_activityNeedsData.Npc.PathFinder.TargetPosition))
                            return;
                        
                        StartNewActivity(GetRandomActivity());
                        return;
                    }
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

            PlacementDataHandler.OnPlacedPositions -= HasPlacementOnTop;
        }
    }
}