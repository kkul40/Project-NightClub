using System.Character.Bartender.Command;
using Prop_Behaviours.Bar;
using ScriptableObjects;
using UI.GamePages;
using UI.PopUp;
using UnityEngine;

namespace PropBehaviours
{
    public class Bar : IPropUnit, IBar
    {
        [SerializeField] private Transform bartenderWaitPosition;
        [SerializeField] private Transform customerWaitPosition;
        [SerializeField] private Transform counterPlacePosition;

        public int InstanceID => GetInstanceID();
        public Transform BartenderWaitPosition => bartenderWaitPosition;
        public Transform CustomerWaitPosition => customerWaitPosition;
        public Transform CounterPlacePosition => counterPlacePosition;
        
        private DrinkTable _drinkTable;
        private bool HasBarCleaned = true;

        public bool HasDrinks
        {
            get
            {
                if (_drinkTable == null) return false;
                if (_drinkTable.drinksLeft <= 0) return false;
                return true;
            }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                IsInteractable = !value;
            }
        }

        public bool IsServing { get; set; }

        public void GetDrink()
        {
            if (!HasDrinks) return;
            
            if (!_drinkTable.IsOutOfDrinks)
            {
                _drinkTable.RemoveDrink(1);
            }
        }

        public void CreateDrinks(DrinkSO drinkToCreate)
        {
            if (HasDrinks) return;
            
            _drinkTable = CreateDrinkTable(drinkToCreate);
            HasBarCleaned = false;
        }
        
        private DrinkTable CreateDrinkTable(DrinkSO drinkData)
        {
            var obj = Instantiate(BarMediator.Instance.DrinkTablePrefab, counterPlacePosition);
            obj.transform.position = counterPlacePosition.position;
            var drinkTable = obj.GetComponent<DrinkTable>();
            drinkTable.SetUpTable(drinkData, this);

            return drinkTable;
        }

        public void CleanBar()
        {
            _drinkTable.CleanUP();
            HasBarCleaned = true;
        }

        public override void OnClick()
        {
            if (HasBarCleaned)
            {
                UIPageManager.Instance.RequestAPage(typeof(UIActionSelectionPage),this);
            }
            else if(_drinkTable.IsOutOfDrinks)
            {
                BarMediator.Instance.AddCommand(this, new CleanDrinkTableCommand());
            }
        }
    }
}