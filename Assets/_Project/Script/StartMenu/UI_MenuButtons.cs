using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartMenu
{
    public class UI_MenuButtons : UI_Page
    {
        public Logger _logger;

        public void OnContinueButton()
        {
            StartGame();
        }

        public void OnNewGameButton(UI_Page page)
        {
            SavingAndLoadingSystem.Instance.NewGame();
            SavingAndLoadingSystem.Instance.LoadGame();
            UI_MainMenuManager.Instance.OpenNewPage(page);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void OnExitButton()
        {
            // _logger.Log("Application Just Quit");
            Application.Quit();
        }
    }
}