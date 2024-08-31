using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class OpenStoreButtonBase : UIButtonBase
    {
        [SerializeField] private UIButtonBase firstStorePageToShow;

        public override void OnClick()
        {
            firstStorePageToShow.OnClick();
        }
    }
}