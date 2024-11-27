using System;
using System.Collections.Generic;
using Data;
using Root;
using TMPro;
using UI;
using UnityEngine;

public class UIGeneralSettingsManager : Singleton<UIGeneralSettingsManager>, ISaveLoad
{
    [Header("Graphics")] 
    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown _resolutionDropDown;
    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolution;
    private int _currentResolutionIndex;

    [Header("Display Mode")] 
    [SerializeField] private TMP_Dropdown _displayModeDropDown;
    private int _currentDisplayModeIndex;
    private FullScreenMode[] _fullScreenModes;
    
    [Header("Quality")]
    [SerializeField] private TMP_Dropdown _qualityDropDown;

    
    [Header("Toggle")]
    [SerializeField] private UIToggleButton EdgeScrollingToggle;
    private bool EdgeScrollingEnabled = false;

    private void Start()
    {
        InitResolution();
        InitFullScreenMode();
        
        _qualityDropDown.ClearOptions();
        
    }

    private void InitFullScreenMode()
    {
        _displayModeDropDown.ClearOptions();
        _fullScreenModes = (FullScreenMode[])Enum.GetValues(typeof(FullScreenMode));

        List<string> options = new List<string>();

        for (int i = 0; i < _fullScreenModes.Length; i++)
        {
            options.Add(_fullScreenModes.GetValue(i).ToString());

            if (_fullScreenModes.GetValue(i).Equals(Screen.fullScreenMode))
            {
                Debug.Log("Equelllsss");
                _currentDisplayModeIndex = i;
            }
        }
      
        _displayModeDropDown.AddOptions(options);
        _displayModeDropDown.value = _currentDisplayModeIndex;
        _displayModeDropDown.RefreshShownValue();
    }

    private void InitResolution()
    {
        _resolutions = Screen.resolutions;
        
        _resolutionDropDown.ClearOptions();
        _filteredResolution = new List<Resolution>();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            _filteredResolution.Add(_resolutions[i]);
        }

        List<string> options = new List<string>();
        for (int i = 0; i < _filteredResolution.Count; i++)
        {
            string resolution = $"{_filteredResolution[i].width} x {_filteredResolution[i].height}";
            options.Add(resolution);

            if (_filteredResolution[i].width == Screen.currentResolution.width && _filteredResolution[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }
        }
        
        _resolutionDropDown.AddOptions(options);
        _resolutionDropDown.value = _currentResolutionIndex;
        _resolutionDropDown.RefreshShownValue();
    }
    
    public void SetScreenResolution(int index)
    {
        Resolution resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    public void SetFullScreenMode(int index)
    {
        Resolution resolution = Screen.currentResolution;
        Screen.SetResolution(resolution.width, resolution.height, _fullScreenModes[index]);
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