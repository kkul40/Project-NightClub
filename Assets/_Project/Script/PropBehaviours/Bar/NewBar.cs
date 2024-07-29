using System;
using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public class NewBar : IPropUnit, IBar
    {
        private BarMediator barMediator;

        [SerializeField] private Transform bartenderWaitPosition;
        [SerializeField] private Transform customerWaitPosition;
        [SerializeField] private Transform counterPlacePosition;
        [SerializeField] private Drink drinkData;


        public Transform BartenderWaitPosition => bartenderWaitPosition;
        public Transform CustomerWaitPosition => customerWaitPosition;
        public Transform CounterPlacePosition => counterPlacePosition;
        public Drink DrinkData => drinkData;
        public bool HasDrinks { get; set; } = false;


        private void Start()
        {
            barMediator = BarMediator.Instance;
        }

        public override void OnClick()
        {
            barMediator.AddCommandToExecute(this, new PrepareDrinkCommand());
        }
    }
}