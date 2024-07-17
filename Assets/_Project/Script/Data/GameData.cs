using System;
using System.Collections.Generic;
using SerializableTypes;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public Vector2Int SavedMapSize;
        public int WallDoorIndexOnX;
        public List<WallSaveData> SavedWallDatas;
        public SerializableDictionary<Vector3Int, FloorSaveData> SavedFloorDatas;

        //CTOR

        public GameData()
        {
            // Map Size
            SavedMapSize = ConstantVariables.InitialMapSize;

            // Wall Door Index
            WallDoorIndexOnX = SavedMapSize.x <= 6 ? SavedMapSize.x - 1 : 6;

            // Wall Data
            var defaltMaterialID = -1;
            SavedWallDatas = new List<WallSaveData>();

            for (var x = 1; x <= SavedMapSize.x; x++)
                SavedWallDatas.Add(new WallSaveData(new Vector3Int(x, 0, 0), defaltMaterialID));

            for (var y = 1; y <= SavedMapSize.y; y++)
                SavedWallDatas.Add(new WallSaveData(new Vector3Int(0, 0, y), defaltMaterialID));

            // Floor Tile Data
            SavedFloorDatas = new SerializableDictionary<Vector3Int, FloorSaveData>();
            for (int x = 0; x < ConstantVariables.MaxMapSizeX; x++)
                for (int y = 0; y < ConstantVariables.MaxMapSizeY; y++)
                    SavedFloorDatas.Add(new Vector3Int(x, 0, y), new FloorSaveData());
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
            
            public WallSaveData(WallAssignmentData wallAssignmentData)
            {
                CellPosition = wallAssignmentData.CellPosition;
                AssignedMaterialID = wallAssignmentData.assignedMaterialID;
            }
        }

        [Serializable]
        public class FloorSaveData
        {
            public Vector3Int CellPosition = -Vector3Int.one;
            public int assignedMaterialID = -890423504;
            public int assignedSurfaceID = -1;
            public int assignedObjectID = -1;

            public FloorSaveData()
            {
            }

            public FloorSaveData(FloorGridAssignmentData floorGridAssignmentData)
            {
                CellPosition = floorGridAssignmentData.CellPosition;
                assignedMaterialID = floorGridAssignmentData.assignedMaterialID;
                assignedSurfaceID = floorGridAssignmentData.assignedSurfaceID;
                assignedObjectID = floorGridAssignmentData.assignedObjectID;
            }
        }
    }
}