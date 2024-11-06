using UI.GamePages.GameButtons;

namespace UI.StartMenu.Buttons
{
    public class LoadGameButtonBase : UIButtonBase
    {
        protected override void Start()
        {
            button.interactable = false;
        }

        public override void OnClick()
        {
            base.OnClick();
        }
    }
}