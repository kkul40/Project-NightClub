using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using BuildingSystem.SO;
using PropBehaviours;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [DisallowMultipleComponent]
    public class DiscoData : Singleton<DiscoData>, ISaveLoad
    {
        public PlacementDataHandler placementDataHandler { get; private set; }
        public MapData MapData => MapGeneratorSystem.Instance.MapData;
        public Inventory inventory;
        public HashSet<StoreItemSO> AllInGameItems { get; private set; }

        public List<IPropUnit> GetPropList => placementDataHandler.GetPropList;

        // !!! FIRST INIT IN THE GAME !!!
        private void Awake()
        {
            inventory = new Inventory();
            placementDataHandler = new PlacementDataHandler();
            AllInGameItems = Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems").ToHashSet();
        }

        public void SaveData()
        {
            SavingAndLoadingSystem.Instance.SaveGame();
        }

        public enum eDanceStyle
        {
            Default,
            Hiphop
        }

        public enum eUISlot
        {
            Slot,
            Cargo
        }

        public void LoadData(GameData gameData)
        {
            placementDataHandler.LoadGameProps(gameData);
            // placementDataHandler.LoadGameProps(gameData);
        }

        public void SaveData(ref GameData gameData)
        {
            placementDataHandler.SaveGameProps(ref gameData);
        }
    }

    public class ConstantVariables
    {
        public const int MaxMapSizeX = 50;
        public const int MaxMapSizeY = 50;
        public const int FloorLayerID = 7;
        public const int WalllayerID = 8;

        public static Vector3 WallObjectOffset = new Vector3(0f, 0f, 0f);
        public static Vector3 PropObjectOffset = new Vector3(0f, -0.5f, 0f);
    }
}