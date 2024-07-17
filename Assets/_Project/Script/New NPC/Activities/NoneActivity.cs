using UnityEngine;

namespace New_NPC.Activities
{
    public class NoneActivity : IActivity
    {
        public bool IsEnded { get; }

        public void StartActivity(ActivityNeedsData and)
        {
        }

        public void UpdateActivity(ActivityNeedsData and)
        {
            Debug.Log("None Activity");
        }

        public void EndActivity(ActivityNeedsData and)
        {
        }
    }
}