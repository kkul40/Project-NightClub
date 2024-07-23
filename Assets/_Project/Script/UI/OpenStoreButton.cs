using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class OpenStoreButton : UIButton
    {
        [SerializeField] private UIButton firtButton;

        private void Awake()
        {
            OnClick();
        }

        public override void OnClick()
        {
            firtButton.OnClick();
        }
    }
}