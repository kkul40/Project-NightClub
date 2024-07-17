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
    public class DiscoData : Singleton<DiscoData>
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
    }

    public class ConstantVariables
    {
        public const int MaxMapSizeX = 50;
        public const int MaxMapSizeY = 50;
        public static readonly Vector2Int InitialMapSize = new(11, 11);
        public const int FloorLayerID = 7;
        public const int WalllayerID = 8;
    }
}