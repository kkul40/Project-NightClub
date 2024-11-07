using System;
using ScriptableObjects;
using UI.GamePages;
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

        public DrinkTable DrinkTable => _drinkTable;

        public bool HasDrinks
        {
            get
            {
                if (_drinkTable == null || _drinkTable.drinkAmount <= 0)
                {
                    return false;
                }
                return true;
            }

            set { }
        }


        public void GetDrink()
        {
            if (HasDrinks)
            {
                _drinkTable.GetDrink();
                return;
            }
        }

        public void CreateDrinks(DrinkSO drinkToCreate)
        {
            if (HasDrinks) return;
            
            _drinkTable = BarMediator.Instance.CreateDrinkTable(this, drinkToCreate);
        }

        public override void OnClick()
        {
            if (_drinkTable != null && _drinkTable.drinkAmount <= 0)
            {
                BarMediator.Instance.AddCommand(this, new CleanDrinkTableCommand());
                return;
            }

            UIPageManager.Instance.RequestAPage(new UIBarPage(), this);
        }
    }
}