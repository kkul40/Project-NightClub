using System.Linq;
using Root;
using SaveAndLoad;
using UI.GamePages.GameButtons;

namespace UI.StartMenu.Buttons
{
    public class ContinueButtonBase : UIButtonBase
    {
        private void OnEnable()
        {
            var saveNames = SaveLoadSystem.Instance.GetList().ToList();
            button.interactable = saveNames.Count > 0;
        }

        public override void OnClick()
        {
            // TODO : Get  the last saved savefile and load that;

            SceneLoader.Instance.LoadScene(1);
            // SceneManager.LoadScene(1);
        }
    }
}