using New_NPC;
using UnityEngine;

namespace PropBehaviours
{
    public class PrepareDrinkCommand : IBartenderCommand
    {
        public IBar bar { get; private set; }
        public NewBartender bartender { get; private set; }
        public bool HasFinish { get; }
        private Transform target;

        private float timer = 0;
        private float prepareTime = 0;

        private int executionOrder = 0;

        public void InitCommand(IBar bar, NewBartender bartender)
        {
            this.bar = bar;
            this.bartender = bartender;
            this.target = bar.BartenderWaitPosition;
        }

        public bool IsDoable()
        {
            if (bar.HasDrinks) return false;
            if (bartender.IsBusy) return false;

            SetThingsBeforeStart();
            return true;
        }

        private void SetThingsBeforeStart()
        {
            bartender.IsBusy = true;
            prepareTime = bar.DrinkData.PrepareTime;
            bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Walk);
        }

        private void SetThingsBeforeExit()
        {
            bartender.IsBusy = false;
        }

        public bool UpdateCommand(BarMediator barMediator)
        {
            switch (executionOrder)
            {
                case 0:
                    while (Vector3.Distance(bartender.transform.position, target.position) > 0.1f)
                    {
                        bartender.transform.position = Vector3.MoveTowards(bartender.transform.position, target.position, Time.deltaTime * 1.5f);
                        return false;
                    }
                    executionOrder++;
                    return false;
                case 1:
                    bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_PrepareDrink);
                    bartender.transform.rotation = target.rotation;
                    executionOrder++;
                    return false;
                case 2:
                    if (timer < prepareTime)
                    {
                        timer += Time.deltaTime;
                        return false;
                    }
                    bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Idle);
                    barMediator.CreateDrinkTable(bar, bar.DrinkData);
                    bar.HasDrinks = true;
                    executionOrder++;
                    return false;
            }

            SetThingsBeforeExit();
            return true;
        }
    }
    
    public class WallToEntranceCommand : IBartenderCommand
    {
        public IBar bar { get; }
        public NewBartender bartender { get; }
        public bool HasFinish { get; }

        public void InitCommand(IBar bar, NewBartender bartender)
        {
        }

        public bool IsDoable()
        {
            return true;
        }

        public bool UpdateCommand(BarMediator barMediator)
        {
            throw new System.NotImplementedException();
        }
    }
}