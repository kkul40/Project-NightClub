using System;

namespace PropBehaviours
{
    public class RemoveDrinksFromTable : IBartenderCommand
    {
        public IBar bar { get; }
        public NewBartender bartender { get; }
        
        public void InitCommand(IBar bar, NewBartender bartender)
        {
            throw new NotImplementedException();
        }

        public bool IsDoable()
        {
            throw new NotImplementedException();
        }

        public bool UpdateCommand(BarMediator barMediator)
        {
            throw new NotImplementedException();
        }
    }
}