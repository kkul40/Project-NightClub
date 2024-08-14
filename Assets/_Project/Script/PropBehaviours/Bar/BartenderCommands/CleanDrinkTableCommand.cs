using PropBehaviours;

namespace New_NPC.Activities
{
    public class CleanDrinkTableCommand : IBartenderCommand
    {
        public IBar bar { get; }
        public IBartender bartender { get; }
        public bool HasFinish { get; }

        public void InitCommand(IBar bar, IBartender bartender)
        {
            throw new System.NotImplementedException();
        }

        public bool IsDoable()
        {
            throw new System.NotImplementedException();
        }

        public void SetThingsBeforeStart()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCommand(BarMediator barMediator)
        {
            throw new System.NotImplementedException();
        }
    }
}