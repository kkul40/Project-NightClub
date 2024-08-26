using System;
using UnityEngine;

namespace PropBehaviours.New_Bar_Logıc
{
    public class NewBar : IPropUnit, IBar
    {
        public int InstanceID { get; private set; }
        public Transform BartenderWaitPosition => bartenderWaitPosition;
        public Transform CustomerWaitPosition => customerWaitPosition;
        public Transform CounterPlacePosition => counterPlacePosition;
        public DrinkTable DrinkTable { get; } = null;
        public int DrinkAmount { get; private set; } = 0;

        [SerializeField] private Transform bartenderWaitPosition;
        [SerializeField] private Transform customerWaitPosition;
        [SerializeField] private Transform counterPlacePosition;


        public bool HasDrinks
        {
            get { return DrinkTable == null ? false : DrinkAmount > 0 ? true : false; }
            set{ }
        }

        private void Start()
        {
            InstanceID = GetHashCode();
        }

        public void GetDrink()
        {
            throw new System.NotImplementedException();
        }

        public void CreateDrinks()
        {
            throw new System.NotImplementedException();
        }

        public bool IsInteractable { get; } = true;
        public eInteraction Interaction { get; } = eInteraction.Basic;
        
        public override void OnFocus()
        {
        }

        public override void OnOutFocus()
        {
        }

        public override void OnClick()
        {
            // Display window and decide which drink to create
        }
    }
}