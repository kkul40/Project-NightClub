namespace NPC_Stuff
{
    public interface IActivity
    {
        bool IsEnded { get; }
        bool CanStartActivity(ActivityNeedsData and);
        void OnActivityStart(ActivityNeedsData and);
        void OnActivityUpdate(ActivityNeedsData and);
        void OnActivityEnd(ActivityNeedsData and);
    }
}