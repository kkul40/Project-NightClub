using Data;
using Root;
using SaveAndLoad;
using UI.GamePages.GameButtons;

namespace UI.StartMenu
{
    public class OnFinishUpCustomizationButtonBase : UIButtonBase
    {
        public override void OnHover()
        {
        }

        public override void OnClick()
        {
            SavingAndLoadingSystem.Instance.NewGame();
            SavingAndLoadingSystem.Instance.SaveGame();
            // SceneManager.LoadScene(1);
            SceneLoader.Instance.LoadScene(1);
        }
    }
}