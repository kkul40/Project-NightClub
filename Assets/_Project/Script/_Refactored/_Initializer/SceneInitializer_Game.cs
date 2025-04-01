using System;
using System.Building;
using System.Character._Player;
using System.Character.NPC;
using System.Music;
using Data;
using GameEvents;
using SaveAndLoad;
using UI;
using UI.GamePages;
using UnityEngine;

namespace _Initializer
{
    public class SceneInitializer_Game : MonoBehaviour
    {
        [SerializeField] private GameEvent gameEvent;
        [SerializeField] private BuildingSystem buildingSystem;
        [SerializeField] private GridSystem gridSystem;
        [SerializeField] private CursorSystem cursorSystem;
        [SerializeField] private DiscoData discoData;
        [SerializeField] private MusicManager musicManager;
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private MapGeneratorSystem mapGeneratorSystem;
        [SerializeField] private Player player;
        [SerializeField] private NPCSystem npcSystem;
        

        // UI Stuff
        [Header("UI Stuff")]
        [SerializeField] private UI_HudManager uiHudManager;
        [SerializeField] private UIPageManager uiPageManager;
        
        private void Awake()
        {
            if (buildingSystem == null)
            {
                Debug.LogError("Building System Is Null");
                return;
            }
            
            if (gridSystem== null)
            {
                Debug.LogError("Building System Is Null"); 
                return;
            }
            
            // Init Data
            // Init Saving Data
            // Generate Map
            // Generate Scene Items

            
            gameEvent.Initialize();
            
            uiHudManager.Initialize();
            uiPageManager.Initialize();

            // SavingAndLoadingSystem.Instance.Initialize();
            // SavingAndLoadingSystem.Instance.NewGame();

            GameData gameData = SavingAndLoadingSystem.GameData;

            
            discoData.Initialize(gameData);
            musicManager.Initialize();
            inputSystem.Initialize();
            
            mapGeneratorSystem.Initialize(discoData.MapData);
            
            gridSystem.Initialize();
            cursorSystem.Initialize(inputSystem, 1);
            buildingSystem.Initialize();
            
            player.Initialize(gameData);
            npcSystem.Initialize(discoData);
            
            // _savingAndLoadingSystem.Initialize(); 
            // _savingAndLoadingSystem.LoadGame();
        }
    }
}