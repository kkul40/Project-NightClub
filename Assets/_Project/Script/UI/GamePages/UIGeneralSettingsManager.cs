using System;
using System.Collections.Generic;
using _Initializer;
using Data;
using DiscoSystem;
using DiscoSystem.Building_System.GameEvents;
using Root;
using SaveAndLoad;
using TMPro;
using UnityEngine;

namespace UI.GamePages
{
    public class UIGeneralSettingsManager : MonoBehaviour
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
        private int _currentGraphicQualityIndex;

    
        [Header("Toggle")]
        [SerializeField] private UIToggleButton EdgeScrollingToggle;
        [SerializeField] private UIToggleButton SnappyCameraToggle;
        private bool EdgeScrollingEnabled = false;
        private bool SnappyCameraEnabled = false;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            InitGeneralSettings();
            
            InitResolution();
            InitFullScreenMode();
            InitQuality();
            
            GameEvent.Subscribe<Event_OnGameSave>(handle => SaveData());
        }

        private void InitGeneralSettings()
        {
            EdgeScrollingEnabled = PlayerPrefs.GetInt("EdgeScrolling", 0) < 1 ? true : false;
            SnappyCameraEnabled = PlayerPrefs.GetInt("SnappyCamera", 0) < 1 ? true : false;
            
            ToggleEdgeScrolling();
            ToggleSnappyCamera();
        }

        private void InitQuality()
        {
            _qualityDropDown.ClearOptions();

            List<string> options = new List<string>()
            {
                "Low",
                "Very Low",
                "Medium",
                "High",
                "Very High",
                "Ultra",
            };
            _qualityDropDown.AddOptions(options);
            _qualityDropDown.value = 5;
            _qualityDropDown.RefreshShownValue();
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
                    _currentDisplayModeIndex = i;
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
                _filteredResolution.Add(_resolutions[i]);

            List<string> options = new List<string>();
            for (int i = 0; i < _filteredResolution.Count; i++)
            {
                string resolution = $"{_filteredResolution[i].width} x {_filteredResolution[i].height}";
                options.Add(resolution);

                if (_filteredResolution[i].width == Screen.currentResolution.width && _filteredResolution[i].height == Screen.currentResolution.height)
                    _currentResolutionIndex = i;
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

        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index, true);
        }

        public void SaveGame()
        {
            ServiceLocator.Get<UIPageManager>().ShowLoadPage();
        }

        public void LoadGame()
        {
            SceneLoader.Instance.LoadScene(1);
        }

        public void MainMenu()
        {
            ServiceLocator.Get<UIPageManager>().ShowAreYouSurePopup("All unsaved progress will be lost. Are you sure you want to continue", () =>
                {
                    SceneLoader.Instance.LoadScene(0);
                });
        }

        public void QuitGame()
        {
            ServiceLocator.Get<UIPageManager>().ShowAreYouSurePopup("All unsaved progress will be lost. Are you sure you want to continue", () =>
                {
                    Application.Quit();
                });
        }

        public void ToggleEdgeScrolling()
        {
            EdgeScrollingEnabled = !EdgeScrollingEnabled;
            ServiceLocator.Get<InputSystem>().EdgeScrolling = EdgeScrollingEnabled;
            EdgeScrollingToggle.ToggleCheckMark(EdgeScrollingEnabled);
        }
    
        public void ToggleSnappyCamera()
        {
            SnappyCameraEnabled = !SnappyCameraEnabled;
            ServiceLocator.Get<InputSystem>().SnappyCamera = SnappyCameraEnabled;
            SnappyCameraToggle.ToggleCheckMark(SnappyCameraEnabled);
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt("EdgeScrolling", EdgeScrollingEnabled ? 1 : 0);
            PlayerPrefs.SetInt("SnappyCamera", SnappyCameraEnabled ? 1 : 0);
        }
    }
}