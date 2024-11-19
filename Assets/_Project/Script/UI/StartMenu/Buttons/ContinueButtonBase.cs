using Data;
using UI.GamePages.GameButtons;
using UnityEngine.SceneManagement;

namespace UI.StartMenu.Buttons
{
    public class ContinueButtonBase : UIButtonBase
    {
        public override void OnAwake()
        {
            button.interactable = SavingAndLoadingSystem.Instance.HasBeenSavedBefore();
        }

        public override void OnClick()
        {
            SceneManager.LoadScene(1);
        }
    }
}