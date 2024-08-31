using Data;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class OnFinishUpCustomizationButtonBase : UIButtonBase
    {
        public override void OnHover()
        {
            throw new System.NotImplementedException();
        }

        public override void OnClick()
        {
            SavingAndLoadingSystem.Instance.NewGame();
            SavingAndLoadingSystem.Instance.SaveGame();
            SceneManager.LoadScene(1);
        }
    }
}