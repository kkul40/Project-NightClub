namespace System.Character.NPC.Activity
{
    public interface IActivity
    {
        bool CheckForPlacementOnTop { get; }
        bool IsEnded { get; }
        bool CanStartActivity(ActivityNeedsData and);
        void OnActivityStart(ActivityNeedsData and);
        void OnActivityUpdate(ActivityNeedsData and);
        void OnActivityEnd(ActivityNeedsData and);
        bool OnActivityErrorHandler(ActivityNeedsData and);
    }
}