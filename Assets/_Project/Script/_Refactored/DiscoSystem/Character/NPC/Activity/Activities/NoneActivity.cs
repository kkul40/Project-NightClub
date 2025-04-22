using UnityEngine;

namespace DiscoSystem.Character.NPC.Activity.Activities
{
    public class NoneActivity : IActivity
    {
        public bool CheckForPlacementOnTop { get; } = false;
        public bool IsEnded { get; }

        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            return false;
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