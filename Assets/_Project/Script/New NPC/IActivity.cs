namespace New_NPC
{
    public interface IActivity
    {
        bool IsEnded { get; }
        void StartActivity(ActivityNeedsData and);
        void UpdateActivity(ActivityNeedsData and);
        void EndActivity(ActivityNeedsData and);
    }
}