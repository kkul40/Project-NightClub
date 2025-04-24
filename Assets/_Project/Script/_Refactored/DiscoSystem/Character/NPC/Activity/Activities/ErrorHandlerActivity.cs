using UnityEngine;

namespace DiscoSystem.Character.NPC.Activity.Activities
{
    public class ErrorHandlerActivity : IActivity
    {
        public bool CheckForPlacementOnTop { get; } = false;
        public bool IsEnded { get; private set; }
        public bool CanStartActivity(ActivityNeedsData and)
        {
            Debug.Log("Error Mode");
            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            // Save If Stuck
            and.Npc.PathFinder.ErrorHelp(() => IsEnded = true);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            Debug.Log("Error Exit");
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            return false;
        }
    }
}