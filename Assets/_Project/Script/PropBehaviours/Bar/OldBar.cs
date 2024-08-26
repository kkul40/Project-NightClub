using System;
using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public class OldBar : IPropUnit
    {
        // private BarMediator barMediator;
        //
        // [SerializeField] private Transform bartenderWaitPosition;
        // [SerializeField] private Transform customerWaitPosition;
        // [SerializeField] private Transform counterPlacePosition;
        // [SerializeField] private Drink drinkData;
        //
        // public int InstanceID => GetInstanceID();
        // public Transform BartenderWaitPosition => bartenderWaitPosition;
        // public Transform CustomerWaitPosition => customerWaitPosition;
        // public Transform CounterPlacePosition => counterPlacePosition;
        // public DrinkTable DrinkTable { get; }
        // public Drink DrinkData => drinkData;
        //
        // public bool HasDrinks
        // {
        //     get
        //     {
        //         if (_drinkTable == null || _drinkTable.drinkAmount <= 0)
        //         {
        //             return false;
        //         }
        //         return true;
        //     }
        //
        //     set { }
        // }
        //
        // private DrinkTable _drinkTable;
        //
        // public void GetDrink()
        // {
        //     if (HasDrinks)
        //     {
        //         _drinkTable.GetDrink();
        //         return;
        //     }
        // }
        //
        // public void CreateDrinks()
        // {
        //     _drinkTable = barMediator.CreateDrinkTable(this);
        // }
        //
        // private void Start()
        // {
        //     barMediator = BarMediator.Instance;
        // }
        //
        // public override void OnClick()
        // {
        //     barMediator.AddCommand(this, new PrepareDrinkCommand());
        // }
    }
}