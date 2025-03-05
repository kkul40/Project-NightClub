using System.Character.NPC;
using Prop_Behaviours.Bar;
using PropBehaviours;
using UnityEngine;

namespace System.Character.Bartender.Command
{
    public class CleanDrinkTableCommand : IBartenderCommand
    {
        public IBar bar { get; private set; }
        public IBartender bartender { get; private set; }
        public bool HasFinish { get; private set; }
        
        private Transform target;
        private eState _state;

        private float timer = 0;
        private float cleanDelay = 5;

        public void InitCommand(IBar bar, IBartender bartender)
        {
            this.bar = bar;
            this.bartender = bartender;
            target = bar.BartenderWaitPosition;
            _state = eState.ReachTarget;
        }

        public bool IsDoable()
        {
            if (bar.HasDrinks) return false;
            if (bar.IsBusy) return false;
            
            bar.IsBusy = true;
            return true;
        }

        public void SetThingsBeforeStart()
        {
            bartender.IsBusy = true;
            bar.IsBusy = true;
            bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Walk);
            bartender.PathFinder.GoTargetDestination(target.position);
        }

        public void UpdateCommand(BarMediator barMediator)
        {
            switch (_state)
            {
                case eState.ReachTarget:
                    if (bartender.PathFinder.HasReachedDestination)
                    {
                        bartender.PathFinder.SetPositioning(bar.BartenderWaitPosition.rotation);
                        bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_CleanUpTable);
                        _state = eState.CleanTable;
                    }
                    break;
                case eState.CleanTable:
                    timer += Time.deltaTime;
                    if (timer > cleanDelay)
                    {
                        bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Idle);
                        bartender.IsBusy = false;
                        bar.IsBusy = false;
                        bar.CleanBar();
                        HasFinish = true;
                    }
                    break;
            }
        }
        
        private enum eState
        {
            ReachTarget,
            CleanTable,
        }
    }
}