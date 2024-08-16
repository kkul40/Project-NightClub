using PropBehaviours;
using UnityEngine;

namespace New_NPC.Activities
{
    public class CleanDrinkTableCommand : IBartenderCommand
    {
        public IBar bar { get; private set; }
        public IBartender bartender { get; private set; }
        public bool HasFinish { get; private set; }
        
        private Transform target;

        private float timer = 0;
        private float cleanDelay = 5;

        private bool startTimer = false;

        public void InitCommand(IBar bar, IBartender bartender)
        {
            this.bar = bar;
            this.bartender = bartender;
            target = bar.BartenderWaitPosition;
        }

        public bool IsDoable()
        {
            if (bar.HasDrinks) return false;
            
            return true;
        }

        public void SetThingsBeforeStart()
        {
            bartender.IsBusy = true;
            bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Walk);
            bartender.PathFinder.GoTargetDestination(target.position, OnReach);
        }

        public void UpdateCommand(BarMediator barMediator)
        {
            if (!startTimer) return;
            
            timer += Time.deltaTime;
            if (timer > cleanDelay)
            {
                bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Idle);
                // MonoBehaviour.Destroy();
                HasFinish = true;
            }
        }

        private void OnReach()
        {
            startTimer = true;
            bartender.PathFinder.SetPositioning(bar.BartenderWaitPosition.rotation);
            bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_CleanUpTable);
        }
    }
}