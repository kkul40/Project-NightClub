using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class OpenStoreButton : UIButton
    {
       [SerializeField] private UIButton firtButton;
        
        public override void OnClick()
        {
            firtButton.OnClick();
        }
    }
}