using New_NPC;
using UnityEngine;

namespace PropBehaviours
{
    public class PrepareDrinkCommand : IBartenderCommand
    {
        public IBar bar { get; private set; }
        public IBartender bartender { get; set; }
        public bool HasFinish { get; private set; } = false;
        private Transform target;

        private float timer = 0;
        private float prepareTime = 0;

        private int executionOrder = 0;

        public void InitCommand(IBar bar, IBartender bartender)
        {
            this.bar = bar;
            this.bartender = bartender;
            this.target = bar.BartenderWaitPosition;
        }

        public bool IsDoable()
        {
            if (bar.HasDrinks)
            {
                Debug.Log("Can't Start Action");
                return false;
            }
            return true;
        }

        public void SetThingsBeforeStart()
        {
            bartender.IsBusy = true;
            prepareTime = bar.DrinkData.PrepareTime;
            bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Walk);
            bartender.PathFinder.GoTargetDestination(target.position);
            Debug.Log("Set Thingss");
        }

        private void SetThingsBeforeExit()
        {
            bar.CreateDrinks();
            bartender.IsBusy = false;
            HasFinish = true;
        }

        public bool UpdateCommand(BarMediator barMediator)
        {
            switch (executionOrder)
            {
                case 0:
                    if (!bartender.PathFinder.HasReachedDestination)
                        return false;
                    executionOrder++;
                    return false;
                case 1:
                    bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_PrepareDrink);
                    bartender.PathFinder.SetRotation(bar.BartenderWaitPosition.rotation);
                    executionOrder++;
                    return false;
                case 2:
                    if (timer < prepareTime)
                    {
                        Debug.Log("Timer");
                        timer += Time.deltaTime;
                        return false;
                    }
                    bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Idle);
                    bar.HasDrinks = true;
                    executionOrder++;
                    return false;
            }

            SetThingsBeforeExit();
            return true;
        }
    }
    
    // public class WallToEntranceCommand : IBartenderCommand
    // {
    //     public IBar bar { get; }
    //     public NewBartender bartender { get; }
    //     public bool HasFinish { get; }
    //
    //     public void InitCommand(IBar bar, NewBartender bartender)
    //     {
    //     }
    //
    //     public bool IsDoable()
    //     {
    //         return true;
    //     }
    //
    //     public bool UpdateCommand(BarMediator barMediator)
    //     {
    //         throw new System.NotImplementedException();
    //     }
    // }
}