namespace PropBehaviours
{
    public interface IBartenderCommand
    {
        IBar bar { get; }
        NewBartender bartender { get; }

        void InitCommand(IBar bar, NewBartender bartender);
        bool IsDoable();
        bool UpdateCommand(BarMediator barMediator);
    }
}