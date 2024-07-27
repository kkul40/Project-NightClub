using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartMenu
{
    public class MenuUIMenuButtons : Menu_UI_Page
    {
        public Logger _logger;

        public void OnContinueButton()
        {
            SceneManager.LoadScene(1);
        }

        public void OnNewGameButton(Menu_UI_Page page)
        {
            UI_MainMenuManager.Instance.OpenNewPage(page);
        }

        public void OnFinishUpCustomizationButton()
        {
            SavingAndLoadingSystem.Instance.NewGame();
            SceneManager.LoadScene(1);
        }

        public void OnExitButton()
        {
            // _logger.Log("Application Just Quit");
            Application.Quit();
        }
    }
}