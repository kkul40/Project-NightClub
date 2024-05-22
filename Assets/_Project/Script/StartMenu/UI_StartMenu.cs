using UnityEngine;

namespace StartMenu
{
    public class UI_StartMenu : UI_Page
    {
        public Logger _logger;


        public void OnNewGameButton(UI_Page page)
        {
            UI_MainMenuManager.Instance.OpenNewPage(page);
            _logger.Log("New Game Button Pressed");
        }

        public void OnExitButton()
        {
            _logger.Log("Application Just Quit");
            Application.Quit();
        }
    }
}