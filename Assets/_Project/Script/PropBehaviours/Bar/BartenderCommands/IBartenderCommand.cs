namespace PropBehaviours
{
    public interface IBartenderCommand
    {
        IBar bar { get; }
        IBartender bartender { get; }

        bool HasFinish { get; }

        void InitCommand(IBar bar, IBartender bartender);
        bool IsDoable();
        void SetThingsBeforeStart();
        void UpdateCommand(BarMediator barMediator);
    }
}