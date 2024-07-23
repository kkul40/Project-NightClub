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
        public PlacementDataHandler placementDataHandler => MapGeneratorSystem.Instance.placementDataHandler;
        public MapData MapData => MapGeneratorSystem.Instance.MapData;
        public Inventory inventory;
        public HashSet<StoreItemSO> AllInGameItems { get; private set; }

        public List<IPropUnit> GetPropList => placementDataHandler.GetPropList;

        // !!! FIRST INIT IN THE GAME !!!
        private void Awake()
        {
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
            ItemSlot,
            InventorySlot,
            ExtentionSlot,
        }

        public void LoadData(GameData gameData)
        {
            inventory = new Inventory(gameData);
            
            Debug.Log("Disco Data Loaded");
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.SavedInventoryDatas.Clear();
            
            foreach (var pair in inventory.Items)
            {
                gameData.SavedInventoryDatas.Add(new GameData.InventorySaveData(pair.Key, pair.Value));
            }
        }

        public StoreItemSO FindItemByID(int ID)
        {
            return DiscoData.Instance.AllInGameItems.FirstOrDefault(x => x.ID == ID);
        }
    }

    public class ConstantVariables
    {
        public const int MaxMapSizeX = 30;
        public const int MaxMapSizeY = 30;
        public const int FloorLayerID = 7;
        public const int WalllayerID = 8;

        public static Vector3 WallObjectOffset = new Vector3(0f, 0f, 0f);
        public static Vector3 PropObjectOffset = new Vector3(0f, -0.5f, 0f);
    }
}