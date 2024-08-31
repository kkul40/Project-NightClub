using UnityEngine;

namespace UI.MainMenu
{
    public class ExitButtonBase : UIButtonBase
    {
        public override void OnClick()
        {
            Application.Quit();
        }
    }
}