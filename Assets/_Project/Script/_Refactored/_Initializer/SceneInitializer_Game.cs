using System;
using System.Collections;
using Data;
using Data.New;
using DiscoSystem;
using DiscoSystem.Building_System;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Character._Player;
using DiscoSystem.Character.NPC;
using SaveAndLoad.New;
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

        private IEnumerator Start()
        {
            _gameEvents.Initialize();
            _musicPlayer.Initialize();
            _inputSystem.Initialize();
            _gridSystem.Initialize();
            _npcSystem.Initialize();


            NewGameData newGameData = SaveLoadSystem.Instance.GetCurrentData();
            
            _discoData.Initialize(newGameData);
            
            yield return StartCoroutine(GameBundle.Instance.InitializeAsync());
            _uiPageManager.Initialize();
            yield return StartCoroutine(_mapGeneratorSystem.InitializeAsync());
            
            _buildingSystem.Initialize();
            _cursorSystem.Initialize(_inputSystem, 1);
            
            _player.Initialize(newGameData);
            
            
            // _savingAndLoadingSystem.Initialize(); 
            // _savingAndLoadingSystem.LoadGame();
        }
    }
}