using UnityEngine;

namespace UI.GamePages.GameButtons
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