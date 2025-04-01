using System;
using System.Building;
using System.Character.Bartender;
using System.Collections.Generic;
using System.Music;
using Data;
using Disco_ScriptableObject;
using PropBehaviours;
using UnityEngine;

namespace GameEvents
{
    #region Game

    public class Event_StartGameSave
    {
        public GameData GameData;

        public Event_StartGameSave(ref GameData gameData)
        {
            GameData = gameData;
        }
    }

    public class Event_GameSaved
    {
        
    }

    #endregion

    #region Building

    public class Event_ToggleBuildingMode
    {
        public bool Toggle;

        public Event_ToggleBuildingMode(bool toggle)
        {
            Toggle = toggle;
        }
    }

    public class Event_PropPlaced
    {
        public IPropUnit PropUnit;
    
        public Event_PropPlaced(IPropUnit propUnit)
        {
            PropUnit = propUnit;
        }
    }
    
    public class Event_PropRemoved
    {
        public IPropUnit PropUnit;
    
        public Event_PropRemoved(IPropUnit propUnit)
        {
            PropUnit = propUnit;
        }
    }

    public class Event_RemovePlacement
    {
        public int InstanceID;

        public Event_RemovePlacement(int instanceID)
        {
            InstanceID = instanceID;
        }
    }

    public class Event_RelocatePlacement
    {
        public int InstanceID;

        public Event_RelocatePlacement(int instanceID)
        {
            InstanceID = instanceID;
        }
    }

    public class Event_PropRelocated
    {
        public IPropUnit ProUnit;

        public Event_PropRelocated(IPropUnit proUnit)
        {
            ProUnit = proUnit;
        }
    }

    public class Event_RelocateWallDoor
    {
        public WallDoor WallDoor;

        public Event_RelocateWallDoor(WallDoor wallDoor)
        {
            WallDoor = wallDoor;
        }
    }
    
    // public class Event_StartPlacement
    // {
    //     public class StorePlacement
    //     {
    //         public StoreItemSO Item;
    //     }
    //
    //     public class InventoryPlacement
    //     {
    //         public int InstanceID;
    //         public int Amount;
    //     }
    //
    //     public class RelocatePlacement
    //     {
    //         public int InstanceID;
    //     }
    //
    //     public class WallDoorPlacement
    //     {
    //         public WallDoor Door;
    //     }
    //
    //
    //     public PlacementMode Mode;
    //
    //     public StorePlacement FromStore;
    //     public InventoryPlacement FromInventory;
    //     public RelocatePlacement Relocate;
    //     public WallDoorPlacement DoorPlacement;
    //
    //
    //     /// <summary>
    //     /// Store Placement
    //     /// </summary>
    //     /// <param name="item"></param>
    //     public Event_StartPlacement(StoreItemSO item)
    //     {
    //         FromStore = new StorePlacement();
    //         FromStore.Item = item;
    //         Mode = PlacementMode.Store;
    //     }
    //     
    //     /// <summary>
    //     /// Inventory Placement
    //     /// </summary>
    //     /// <param name="instanceID"></param>
    //     /// <param name="amount"></param>
    //     public Event_StartPlacement(int instanceID, int amount)
    //     {
    //         FromInventory = new InventoryPlacement();
    //         FromInventory.InstanceID = instanceID;
    //         FromInventory.Amount = amount;
    //         Mode = PlacementMode.Inventory;
    //     }
    //
    //     /// <summary>
    //     /// Relocate Placement
    //     /// </summary>
    //     /// <param name="instanceID"></param>
    //     public Event_StartPlacement(int instanceID)
    //     {
    //         Relocate = new RelocatePlacement();
    //         Relocate.InstanceID = instanceID;
    //         Mode = PlacementMode.Relocating;
    //     }
    //
    //     /// <summary>
    //     /// Wall Door Placement
    //     /// </summary>
    //     /// <param name="wallDoor"></param>
    //     public Event_StartPlacement(WallDoor wallDoor)
    //     {
    //         DoorPlacement = new WallDoorPlacement();
    //         DoorPlacement.Door = wallDoor;
    //         Mode = PlacementMode.RelocatingDoor;
    //     }
    // }
    

    #endregion
    
    #region Sound
    public class Event_Sfx
    {
        public SoundFXType FXType;
        public float Volume;
        public bool Delay;

        public Event_Sfx(SoundFXType fxType, float volume = 1f, bool delay = false)
        {
            FXType = fxType;
            Volume = Mathf.Clamp(volume,0,1);
            Delay = delay;
        }

        public Event_Sfx(SoundFXType fxType, bool delay)
        {
            FXType = fxType;
            Volume = 1;
            Delay = delay;
        }
    }

    public class Event_PlaySong
    {
        public AudioClip Clip;

        public Event_PlaySong(AudioClip clip)
        {
            Clip = clip;
        }
    }

    public class Event_StopSong
    {
        
    }

    public enum SourceVolume
    {
        Master,
        Music,
        Sfx,
    }

    public class Event_SetVolume
    {
        public SourceVolume SourceVolume;
        public float Volume;

        public Event_SetVolume(SourceVolume sourceVolume, float volume)
        {
            SourceVolume = sourceVolume;
            Volume = volume;
        }
    }
    
    #endregion

    #region Cursor

    public class Event_SelectCursor
    {
        public eCursorTypes CursorType;

        public Event_SelectCursor(eCursorTypes cursorType)
        {
            CursorType = cursorType;
        }
    }

    public class Event_ResetSelection
    {
        
    }

    public class Event_ResetCursor
    {
    }

    public class Event_PreviousCursor
    {
    }

    #endregion

    #region Map

    
    public class Event_ExpendMapSize
    {
        public int X;
        public int Y;
        public Vector2Int Size => new Vector2Int(X, Y);
        public Event_ExpendMapSize(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Event_ExpendMapSize(Vector2Int vector2Int)
        {
            X = vector2Int.x;
            Y = vector2Int.y;
        }
    }

    public class Event_MapSizeChanged
    {
        public int X;
        public int Y;
        
        public Vector2Int Size => new Vector2Int(X, Y);
        
        public Event_MapSizeChanged(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public Event_MapSizeChanged(Vector2Int vector2Int)
        {
            X = vector2Int.x;
            Y = vector2Int.y;
        }

    }
    
    #endregion

    #region Employee

    public class Event_BartenderHired
    {
        public IBartender Bartender;

        public Event_BartenderHired(IBartender bartender)
        {
            Bartender = bartender;
        }
    }
    
    public class Event_BartenderKicked
    {
        public IBartender Bartender;

        public Event_BartenderKicked(IBartender bartender)
        {
            Bartender = bartender;
        }
    }

    #endregion

    #region Inventory

    public class Event_BalanceUpdated
    {
        public int Balance;

        public Event_BalanceUpdated(int balance)
        {
            Balance = balance;
        }
    }

    public class Event_MoneyAdded
    {
        public int Amount;

        public Event_MoneyAdded(int amount)
        {
            Amount = amount;
        }
    }
    
    public class Event_RemoveMoney
    {
        public int Amount;
        public bool PlaySfx;

        public Event_RemoveMoney(int amount, bool playSfx)
        {
            Amount = amount;
            PlaySfx = playSfx;
        }
    }
    
    public class Event_InventoryItemsUpdated
    {
        public Dictionary<StoreItemSO, int> InventoryItem;
        
        public Event_InventoryItemsUpdated(Dictionary<StoreItemSO, int> items)
        {
            InventoryItem = items;
        }
    }

    public class Event_AddItem
    {
        public StoreItemSO Item;
        public int Amount;

        public Event_AddItem(StoreItemSO item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }

    public class Event_RemoveItem
    {
        public StoreItemSO Item;
        public int Amount;

        public Event_RemoveItem(StoreItemSO item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }

    #endregion
}