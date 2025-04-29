using System.Linq;
using SaveAndLoad.New;
using UI.GamePages.GameButtons;

namespace UI.StartMenu.Buttons
{
    public class LoadGameButtonBase : UIButtonBase
    {
        public Menu_UI_Page page;

        protected override void Start()
        {
            var saveNames = SaveLoadSystem.Instance.GetList().ToList();
            button.interactable = saveNames.Count > 0 ? true : false;
        }

        public override void OnClick()
        {
            base.OnClick();
            UI_MainMenuManager.Instance.OpenNewPage(page);
        }
    }
}