using System;
using Data;
using DiscoSystem;
using DiscoSystem.Building_System;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Character._Player;
using DiscoSystem.Character.NPC;
using SaveAndLoad;
using UI.GamePages;
using UnityEngine;

namespace _Initializer
{
    [DefaultExecutionOrder(-50)]
    public class SceneInitializer_Game : MonoBehaviour
    {
        [SerializeField] private BuildingSystem _buildingSystem;
        [SerializeField] private UIPageManager _uiPageManager;
        [SerializeField] private GridSystem _gridSystem;
        [SerializeField] private CursorSystem _cursorSystem;
        [SerializeField] private DiscoData _discoData;
        [SerializeField] private MusicPlayer _musicPlayer;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private MapGeneratorSystem _mapGeneratorSystem;
        [SerializeField] private Player _player;
        [SerializeField] private NPCSystem _npcSystem;
        [SerializeField] private GameEvent _gameEvents;
        

        private void Awake()
        {
            if (_buildingSystem == null)
            {
                Debug.LogError("Building System Is Null");
                return;
            }
            
            if (_gridSystem== null)
            {
                Debug.LogError("Building System Is Null"); 
                return;
            }
            
            
            // Init Data
            // Init Saving Data
            // Generate Map
            // Generate Scene Items

            _gameEvents.Initialize();

            
            SavingAndLoadingSystem.Instance.Initialize();
            SavingAndLoadingSystem.Instance.NewGame();

            GameData gameData = SavingAndLoadingSystem.GameData;

            
            _discoData.Initialize(gameData);
            _musicPlayer.Initialize();
            _inputSystem.Initialize();
            
            _mapGeneratorSystem.Initialize(_discoData.MapData);
            
            _gridSystem.Initialize();
            _cursorSystem.Initialize(_inputSystem, 1);
            _buildingSystem.Initialize();
            _uiPageManager.Initialize();
            
            
            _player.Initialize(gameData);
            _npcSystem.Initialize(_discoData);
            
            // _savingAndLoadingSystem.Initialize(); 
            // _savingAndLoadingSystem.LoadGame();
        }
    }
}