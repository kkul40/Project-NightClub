using Data;
using NPCBehaviour;
using Prop_Behaviours.Bar;
using ScriptableObjects;
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
        private DrinkSO _drinkSoToPrepare;

        private eState _state = eState.ReachTarget;

        public PrepareDrinkCommand(DrinkSO drinkSoToPrepare)
        {
            _drinkSoToPrepare = drinkSoToPrepare;
        }

        public void InitCommand(IBar bar, IBartender bartender)
        {
            this.bar = bar;
            this.bartender = bartender;
            target = bar.BartenderWaitPosition;
        }

        public bool IsDoable()
        {
            if (bar.HasDrinks)
            {
                Debug.Log("Can't Start Action");
                return false;
            }
            if (bar.IsBusy) return false;

            bar.IsBusy = true;
            return true;
        }

        public void SetThingsBeforeStart()
        {
            bartender.IsBusy = true;
            bar.IsBusy = true;
            prepareTime = _drinkSoToPrepare.PrepareTime;
            bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Walk);
            bartender.PathFinder.GoTargetDestination(target.position);
        }

        public void UpdateCommand(BarMediator barMediator)
        {
            if(bar == null) bartender.RemoveCommand(); 
            
            switch (_state)
            {
                case eState.ReachTarget:
                    if (!bartender.PathFinder.HasReachedDestination)
                        break;

                    bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_PrepareDrink);
                    bartender.PathFinder.SetPositioning(bar.BartenderWaitPosition.rotation);
                    _state = eState.PrepareDrink;
                    break;
                case eState.PrepareDrink:
                    if (timer < prepareTime)
                    {
                        timer += Time.deltaTime;
                        break;
                    }

                    bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Idle);
                    bar.CreateDrinks(_drinkSoToPrepare);
                    bartender.IsBusy = false;
                    bar.IsBusy = false;
                    HasFinish = true;
                    break;
            }
        }

        private enum eState
        {
            ReachTarget,
            PrepareDrink
        }
    }

    public class WalkToEntranceCommand : IBartenderCommand
    {
        public IBar bar { get; }
        public IBartender bartender { get; private set; }
        public bool HasFinish { get; private set; }

        public void InitCommand(IBar bar, IBartender bartender)
        {
            this.bartender = bartender;
        }

        public bool IsDoable()
        {
            return true;
        }

        public void SetThingsBeforeStart()
        {
            bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Walk);
            bartender.PathFinder.GoTargetDestination(DiscoData.Instance.MapData.EnterencePosition());
        }

        public void UpdateCommand(BarMediator barMediator)
        {
            if (bartender.PathFinder.HasReachedDestination)
            {
                bartender.AnimationController.PlayAnimation(eAnimationType.Bartender_Idle);
                HasFinish = true;
            }
        }
    }
}