using System;
using Data;
using Root;
using UI;
using UnityEngine;

public class UIGeneralSettingsManager : Singleton<UIGeneralSettingsManager>, ISaveLoad
{
    [Header("Toggle")]
    [SerializeField] private UIToggleButton EdgeScrollingToggle;
    private bool EdgeScrollingEnabled = false;

    private void Start()
    {
        // EdgeScrollingToggle.ToggleCheckMark(EdgeScrollingEnabled);
        // EdgeScrollingToggle.AddToggleListener(() => ToggleEdgeScrolling());
    }

    public void SaveGame()
    {
        SavingAndLoadingSystem.Instance.SaveGame();
    }

    public void LoadGame()
    {
        SceneLoader.Instance.LoadScene(1);
        // SceneManager.LoadScene(1);
        // SavingAndLoadingSystem.Instance.LoadGame();
    }

    public void MainMenu()
    {
        SceneLoader.Instance.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleEdgeScrolling()
    {
        EdgeScrollingEnabled = !EdgeScrollingEnabled;
        InputSystem.Instance.EdgeScrolling = EdgeScrollingEnabled;
        EdgeScrollingToggle.ToggleCheckMark(EdgeScrollingEnabled);
    }

    public void LoadData(GameData gameData)
    {
        EdgeScrollingEnabled = !gameData.GameSettingsData.isEdgeScrollingEnabled;
        ToggleEdgeScrolling();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.GameSettingsData.isEdgeScrollingEnabled = EdgeScrollingEnabled;
    }
}