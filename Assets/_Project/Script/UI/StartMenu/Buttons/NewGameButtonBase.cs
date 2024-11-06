using UI.GamePages.GameButtons;

namespace UI.StartMenu.Buttons
{
    public class NewGameButtonBase : UIButtonBase
    {
        public Menu_UI_Page page;

        protected override void Start()
        {
        }

        public override void OnHover()
        {
        }

        public override void OnClick()
        {
            UI_MainMenuManager.Instance.OpenNewPage(page);
        }
    }
}