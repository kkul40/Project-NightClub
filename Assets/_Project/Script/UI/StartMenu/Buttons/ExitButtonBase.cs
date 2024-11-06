using UI.GamePages.GameButtons;
using UnityEngine;

namespace UI.StartMenu.Buttons
{
    public class ExitButtonBase : UIButtonBase
    {
        public override void OnClick()
        {
            Application.Quit();
        }
    }
}