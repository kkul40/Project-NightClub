using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class OpenStoreButton : UIButton
    {
        [SerializeField] private UIButton firstStorePageToShow;

        public override void OnClick()
        {
            firstStorePageToShow.OnClick();
        }
    }
}