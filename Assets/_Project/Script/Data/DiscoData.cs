using System;
using System.Collections.Generic;
using System.IO;
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
        [SerializeField] private bool CreateDefaultSaveDataOnStart;
        private SavingAndLoadingSystem SavingSystem;
        public PlacementDataHandler placementDataHandler { get; private set; }
        public MapData mapData;
        public Inventory inventory;
        public HashSet<StoreItemSO> AllInGameItems { get; private set; }
        
        public List<IPropUnit> GetPropList => placementDataHandler.GetPropList;

        // !!! FIRST INIT IN THE GAME !!!
        private void Awake()
        {
            SavingSystem = new SavingAndLoadingSystem(CreateDefaultSaveDataOnStart);
            mapData = new MapData(SavingSystem.GetSaveData);
            inventory = new Inventory();
            placementDataHandler = new PlacementDataHandler();
            AllInGameItems = Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems").ToHashSet();
        }

        public void SaveData()
        {
            SavingSystem.SaveGame();
        }

        public class MapData
        {
            public Vector2Int CurrentMapSize = Vector2Int.zero;
            // TODO Make door ps vector2Int
            public Vector3Int DoorPosition { get; private set; }

            public PathFinderNode[,] PathFinderNodes { get; }
            public FloorGridAssignmentData[,] FloorGridDatas { get; }
            public List<WallAssignmentData> WallDatas { get; set; }
            public int WallDoorIndex { get; private set; }

            // TODO Bu Verileri SaveData dan cek
            public MapData(SaveData saveData)
            {
                // Load Data
                LoadMapData(saveData);

                PathFinderNodes = new PathFinderNode[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
                FloorGridDatas = new FloorGridAssignmentData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
                
                for (int i = 0; i < ConstantVariables.MaxMapSizeX; i++)
                    for (int j = 0; j < ConstantVariables.MaxMapSizeY; j++)
                    {
                        PathFinderNodes[i, j] = new PathFinderNode(false, -Vector3.one, -1, -1);
                        FloorGridDatas[i, j] = new FloorGridAssignmentData(-Vector3Int.one);
                    }
            }

            private void LoadMapData(SaveData saveData)
            {
                CurrentMapSize = saveData.SavedMapSize;
                WallDoorIndex = saveData.WallDoorIndexOnX;
                DoorPosition = new Vector3Int(WallDoorIndex, 0, -1);
                
                WallDatas = new List<WallAssignmentData>();
                foreach (var saveD in saveData.SavedWallDatas)
                {
                    Debug.Log(saveD.AssignedMaterialID);
                    WallDatas.Add(new WallAssignmentData(saveD.CellPosition, saveD.AssignedMaterialID));
                }
            }

            public bool SetCurrentMapSize(MapGeneratorSystem mapGeneratorSystem, Vector2Int mapSize)
            {
                CurrentMapSize = mapSize;
                return true;
            }
            
            public PathFinderNode SetTileNodeByCellPos(Vector3Int cellpos)
            {
                if (cellpos.x > CurrentMapSize.x || cellpos.z > CurrentMapSize.y)
                {
                    Debug.LogError("TileNode Index Is Not Valid");
                    return null;
                }
                return PathFinderNodes[cellpos.x, cellpos.z];
            }
            
            public Vector3 EnterencePosition => GridHandler.Instance.GetCellCenterWorld(DoorPosition);
        }
        
        public class ConstantVariables
        {
            public const int MaxMapSizeX = 50;
            public const int MaxMapSizeY = 50;
            public static readonly Vector2Int InitialMapSize = new Vector2Int(11, 11);
            public const int FloorLayerID = 7;
            public const int WalllayerID = 8;
        }
        public enum eDanceStyle
        {
            Default,
            Hiphop,
        }
        public enum eUISlot
        {
            Slot,
            Cargo,
        }
    }
    
    public class SavingAndLoadingSystem
    {
        string path = Application.persistentDataPath + "/PlayerGameData.kdata";

        private SaveData CurrentSaveData = SaveData.GetDefaultSaveData();
        public SaveData GetSaveData
        {
            get => CurrentSaveData;
        }

        public SavingAndLoadingSystem(bool CreateDefaultSave)
        {
            Debug.Log(path);
            LoadGame();
        }
            
        public SaveData SaveGame()
        {
            SaveData data = new SaveData();
         
            data.SavedMapSize = DiscoData.Instance.mapData.CurrentMapSize;
            data.WallDoorIndexOnX = DiscoData.Instance.mapData.WallDoorIndex;
            
            var temp = DiscoData.Instance.mapData.WallDatas.ToList();
            for (int i = 0; i < temp.Count; i++)
                data.SavedWallDatas.Add(new SaveData.WallSaveData(temp[i].CellPosition, temp[i].assignedMaterialID));
            
            
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
            
            PrintOut(data);
            
            CurrentSaveData = data;
            Debug.Log("...Game is Saved...");
            
            return data;
        }

        public SaveData LoadGame()
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<SaveData>(json);
                
                PrintOut(data);

                CurrentSaveData = data;
                Debug.Log("...Game is Loaded...");
                return data;
            }
                
            CurrentSaveData = SaveData.GetDefaultSaveData();
            PrintOut(CurrentSaveData);
            
            return CurrentSaveData;
        }

        private void PrintOut(SaveData saveData)
        {
            // Debug.Log("Loaded Map Size : " + CurrentSaveData.SavedMapSize);
            // Debug.Log("Loaded Wall count : " + CurrentSaveData.SavedWallDatas.Count);
        }
    }
    
    [Serializable]
    public class SaveData
    {
        public Vector2Int SavedMapSize = -Vector2Int.one;
        public int WallDoorIndexOnX = -1;
        public List<WallSaveData> SavedWallDatas = new List<WallSaveData>();
        
        //CTOR

        public static SaveData GetDefaultSaveData()
        {
            SaveData dataOut = new SaveData();
            
            // Map Size
            dataOut.SavedMapSize = DiscoData.ConstantVariables.InitialMapSize;
            
            // Wall Door Index
            dataOut.WallDoorIndexOnX = dataOut.SavedMapSize.x <= 6 ? dataOut.SavedMapSize.x - 1 : 6;
            
            // Wall Data
            int defaltMaterialID = -1;
            dataOut.SavedWallDatas = new List<WallSaveData>();

            for (int x = 1; x <= dataOut.SavedMapSize.x; x++)
            {
                dataOut.SavedWallDatas.Add(new WallSaveData(new Vector3Int(x, 0, 0), defaltMaterialID));
            }
            
            for (int y = 1; y <= dataOut.SavedMapSize.y; y++)
                dataOut.SavedWallDatas.Add(new WallSaveData(new Vector3Int(0, 0, y), defaltMaterialID));
            
            // Floor Tile Data
            
            Debug.Log("Default Save Data Loaded");
            return dataOut;
        }
      
        [Serializable]
        public class WallSaveData
        {
            public Vector3Int CellPosition = -Vector3Int.one;
            public int AssignedMaterialID = -1;
            
            public WallSaveData(Vector3Int cellPosition, int assignedMaterialID)
            {
                CellPosition = cellPosition;
                AssignedMaterialID = assignedMaterialID;
            }
        }

        [Serializable]
        public class FloorSaveData
        {
            public Vector3Int CellPosition = -Vector3Int.one;
            public int assignedMaterialID = -1;
            public int assignedSurfaceID = -1;
            public int assignedObjectID = -1;
        }
    }
}