using UnityEngine;

namespace New_NPC.Activities
{
    public class NoneActivity : IActivity
    {
        public bool IsEnded { get; }

        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            Debug.Log("None Activity");
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
        }
    }
}