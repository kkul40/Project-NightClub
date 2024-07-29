using System;
using Data;
using New_NPC.Activities;
using UnityEngine;

namespace New_NPC
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

            _currentActivity = new WalkRandomActivity();
            _lastActivity = _currentActivity;
            _currentActivity.OnActivityStart(_activityNeedsData);
        }

        public void StartNewActivity(IActivity activity)
        {
            if (!activity.CanStartActivity(_activityNeedsData)) return;

            if (hasActivity) _currentActivity.OnActivityEnd(_activityNeedsData);

            _currentActivity = activity;
            _lastActivity = _currentActivity;
            _currentActivity.OnActivityStart(_activityNeedsData);
        }

        public void UpdateActivity()
        {
            if (!hasActivity) return;

            if (_currentActivity.IsEnded)
            {
                GetRandomActivity();
                return;
            }

            _currentActivity.OnActivityUpdate(_activityNeedsData);
        }

        private void GetRandomActivity()
        {
            var randomActivity = ActivitySystem.Instance.GetRandomActivity();

            var iterate = 0;
            while (_lastActivity.GetType() == randomActivity.GetType())
            {
                randomActivity = ActivitySystem.Instance.GetRandomActivity();

                iterate++;
                if (iterate > 100)
                {
                    randomActivity = new ExitDiscoActivity();
                    Debug.LogError("Could Not Found New Activity");
                    break;
                }
            }

            StartNewActivity(randomActivity);
        }
    }
}