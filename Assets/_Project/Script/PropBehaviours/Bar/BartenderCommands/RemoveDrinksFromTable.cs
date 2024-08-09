using System;

namespace PropBehaviours
{
    public class RemoveDrinksFromTable : IBartenderCommand
    {
        public IBar bar { get; }
        public IBartender bartender { get; }
        public bool HasFinish { get; }

        public void InitCommand(IBar bar, IBartender bartender)
        {
            throw new NotImplementedException();
        }

        public bool IsDoable()
        {
            throw new NotImplementedException();
        }

        public void SetThingsBeforeStart()
        {
            throw new NotImplementedException();
        }

        public bool UpdateCommand(BarMediator barMediator)
        {
            throw new NotImplementedException();
        }
    }
}