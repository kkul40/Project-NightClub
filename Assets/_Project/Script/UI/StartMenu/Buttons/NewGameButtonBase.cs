using StartMenu;

namespace UI.MainMenu
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