using System;
using System.Collections;
using Data;
using New_NPC;
using UnityEngine;

namespace PropBehaviours
{
    public class NewBartender : MonoBehaviour, IInteractable
    {
        private BarMediator _barMediator;

        public IAnimationController AnimationController;


        public eInteraction Interaction { get; }

        public bool IsBusy = false;


        private void Start()
        {
            _barMediator = BarMediator.Instance;
            AnimationController = new BartenderAnimationControl(GetComponent<Animator>(),
                InitConfig.Instance.GetDefaultBartenderAnimation, transform.GetChild(0));
        }

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
        }
    }
}