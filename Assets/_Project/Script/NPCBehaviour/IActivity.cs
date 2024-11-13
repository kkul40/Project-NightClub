namespace NPCBehaviour
{
    public interface IActivity
    {
        bool CheckForPlacementOnTop { get; }
        bool IsEnded { get; }
        bool CanStartActivity(ActivityNeedsData and);
        bool ForceToQuitActivity(ActivityNeedsData and);
        void OnActivityStart(ActivityNeedsData and);
        void OnActivityUpdate(ActivityNeedsData and);
        void OnActivityEnd(ActivityNeedsData and);
    }
}