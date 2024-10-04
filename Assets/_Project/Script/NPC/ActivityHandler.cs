using System;
using Data;
using DefaultNamespace;
using NPC_Stuff.Activities;
using UnityEngine;

namespace NPC_Stuff
{
    public class ActivityHandler
    {
        private ActivityNeedsData _activityNeedsData;
        private IActivity _lastActivity;
        private IActivity _currentActivity;

        public bool hasActivity => _currentActivity != null;

        public ActivityHandler(NPC npc)
        {
            _activityNeedsData = new ActivityNeedsData();
            _activityNeedsData.Npc = npc;
            _activityNeedsData.DiscoData = DiscoData.Instance;
            _activityNeedsData.GridHandler = GridHandler.Instance;

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
    }
}