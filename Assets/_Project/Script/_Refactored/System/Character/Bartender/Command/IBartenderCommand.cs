using Prop_Behaviours.Bar;
using PropBehaviours;

namespace System.Character.Bartender.Command
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