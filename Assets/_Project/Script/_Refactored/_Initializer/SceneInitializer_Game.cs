using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Data.New;
using DiscoSystem;
using DiscoSystem.Building_System;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Character._Player;
using DiscoSystem.Character.NPC;
using DiscoSystem.MusicPlayer;
using SaveAndLoad;
using UI.GamePages;
using UnityEngine;

namespace _Initializer
{
    public static class ServiceLocator
    {
        public static Dictionary<Type, object> services = new Dictionary<Type, object>();
        public static T Get<T>()
        {
            if (services.TryGetValue(typeof(T), out var service))
                return (T)service;
            throw new Exception($"Service {typeof(T)} not found");
        }
        public static void Register<T>(T service) => services[typeof(T)] = service;
        public static void Clear() => services.Clear();
    }
    
    [DefaultExecutionOrder(0)]
    public class SceneInitializer_Game : MonoBehaviour
    {
        [SerializeField] private BuildingSystem _buildingSystem;
        [SerializeField] private UIPageManager _uiPageManager;
        [SerializeField] private GridSystem _gridSystem;
        [SerializeField] private CursorSystem _cursorSystem;
        [SerializeField] private DiscoData _discoData;
        [SerializeField] private MusicPlayer _musicPlayer;
        [SerializeField] private SFXPlayer _sfxPlayer;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private MapGeneratorSystem _mapGeneratorSystem;
        [SerializeField] private Player _player;
        [SerializeField] private NPCSystem _npcSystem;
        [SerializeField] private GameEvent _gameEvents;

        private void Awake()
        {
            ServiceLocator.Clear();
            StartCoroutine(InitializeGame());
        }

        private IEnumerator InitializeGame()
        {
            _gameEvents.Initialize();
            
            _musicPlayer.Initialize();
            _sfxPlayer.Initialize();
            _inputSystem.Initialize();
            _gridSystem.Initialize();
            _npcSystem.Initialize();
            _cursorSystem.Initialize(_inputSystem, 0);

            NewGameData newGameData = SaveLoadSystem.Instance.GetCurrentData();
            
            Debug.Log(newGameData.fileName);

            _discoData.Initialize(newGameData);
            
            _uiPageManager.Initialize();
            yield return StartCoroutine(GameBundle.Instance.InitializeAsync());
            Debug.Log("Asset Bundle Really Downloaded");
            yield return StartCoroutine(_mapGeneratorSystem.InitializeAsync());
            _buildingSystem.Initialize();
            
            _player.Initialize(newGameData, _discoData);
            
            
            // _savingAndLoadingSystem.Initialize(); 
            // _savingAndLoadingSystem.LoadGame();
        }
    }
}