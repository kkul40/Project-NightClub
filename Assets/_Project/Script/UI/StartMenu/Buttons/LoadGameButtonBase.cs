using System.Linq;
using SaveAndLoad;
using UI.GamePages.GameButtons;

namespace UI.StartMenu.Buttons
{
    public class LoadGameButtonBase : UIButtonBase
    {
        public Menu_UI_Page page;

        private void OnEnable()
        {
            var saveNames = SaveLoadSystem.Instance.GetList().ToList();
            button.interactable = saveNames.Count > 0;
        }

        public override void OnClick()
        {
            base.OnClick();
            UI_MainMenuManager.Instance.OpenNewPage(page);
        }
    }
}