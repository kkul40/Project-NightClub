using System;
using Data;

namespace New_NPC
{
    public class ActivityHandler
    {
        private ActivityNeedsData _activityNeedsData;
        private IActivity _currentActivity;

        public bool hasActivity => _currentActivity != null;
        
        public ActivityHandler(NPC npc)
        {
            _activityNeedsData = new ActivityNeedsData();
            _activityNeedsData.Npc = npc;
            _activityNeedsData.DiscoData = DiscoData.Instance;
            _activityNeedsData.GridHandler = GridHandler.Instance;
        }

        public void StartActivity(IActivity activity)
        {
            if (hasActivity)
                _currentActivity.EndActivity(_activityNeedsData);
            
            _currentActivity = activity;
            _currentActivity.StartActivity(_activityNeedsData);
        }

        public void UpdateActivity()
        {
            if (hasActivity)
            {
                if (_currentActivity.IsEnded)
                {
                    _currentActivity.EndActivity(_activityNeedsData);
                    StartActivity(ActivitySystem.Instance.GetRandomActivity());
                }
                else
                {
                    _currentActivity.UpdateActivity(_activityNeedsData);
                }
            }
        }
    }
}