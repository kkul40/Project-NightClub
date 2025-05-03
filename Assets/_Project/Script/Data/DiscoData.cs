using System;
using System.Collections;
using System.Collections.Generic;
using Data.New;
using DiscoSystem.Building_System.GameEvents;
using PropBehaviours;
using UnityEngine;

namespace Data
{
    [DisallowMultipleComponent]
    public class DiscoData : MonoBehaviour
    {
        public static DiscoData Instance;

        public MapData MapData;
        public Inventory inventory;
        
        //            instanceID   StoreID created-obj   Pos      Rot
        public Dictionary<int, Tuple<int, Transform, Vector3, Quaternion>> PlacedItems;

        public void Initialize(NewGameData gameData)
        {
            Instance = this;
            PlacedItems = new Dictionary<int, Tuple<int, Transform, Vector3, Quaternion>>();
            MapData = new MapData(gameData);
            inventory = new Inventory(gameData);
        }
    
        private void Start()
        {
            GameEvent.Trigger(new Event_BalanceUpdated(inventory.Balance));
            GameEvent.Trigger(new Event_InventoryItemsUpdated(inventory.Items));
            GameEvent.Trigger(new Event_MapSizeChanged(MapData.CurrentMapSize.x, MapData.CurrentMapSize.y));
        }

        private void OnDisable()
        {
            inventory.Dispose();
            MapData.Dispose();
        }

        public List<T> GetPlacedPropsByType<T>()
        {
            List<T> output = new List<T>();
            foreach (var value in PlacedItems.Values)
            {
                if (value.Item2.TryGetComponent(out IPropUnit unit))
                {
                    if (unit is T t)
                    {
                        output.Add(t);
                    }
                }
            }

            return output;
        }

        public List<IPropUnit> GetPropList()
        {
            List<IPropUnit> output = new List<IPropUnit>();
            foreach (var value in PlacedItems.Values)
            {
                if (value.Item2.TryGetComponent(out IPropUnit unit))
                {
                    output.Add(unit);
                }
            }

            return output;
        }
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
        ExtentionSlot
    }
    
    public enum ePlacementLayer
    {
        BaseSurface, // General BaseSurface Placement
        FloorProp, // Objects placed on the Floor
        WallProp, // Objects placed ont the Wall
        Null
    }
    
    public enum eMaterialLayer
    {
        FloorMaterial,
        WallMaterial,
        Null
    }

    public class ConstantVariables
    {
        public const int MaxMapSizeX = 50;
        public const int MaxMapSizeY = 50;
        public const int PathFinderGridSize = 6;
        public const int DoorHeight = 2;
        public const int FloorLayerID = 7;
        public const int WalllayerID = 8;
    }
}