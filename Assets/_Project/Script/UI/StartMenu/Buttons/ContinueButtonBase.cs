using Data;
using UI.GamePages.GameButtons;
using UnityEngine.SceneManagement;

namespace UI.StartMenu.Buttons
{
    public class ContinueButtonBase : UIButtonBase
    {
        protected override void Start()
        {
            button.interactable = SavingAndLoadingSystem.Instance.HasBeenSavedBefore();
        }

        public override void OnClick()
        {
            SceneManager.LoadScene(1);
        }
    }
}