using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public Vector2Int SavedMapSize = -Vector2Int.one;
        public int WallDoorIndexOnX = -1;
        public List<WallSaveData> SavedWallDatas = new();

        //CTOR

        public static GameData GetDefaultSaveData()
        {
            var dataOut = new GameData();

            // Map Size
            dataOut.SavedMapSize = ConstantVariables.InitialMapSize;

            // Wall Door Index
            dataOut.WallDoorIndexOnX = dataOut.SavedMapSize.x <= 6 ? dataOut.SavedMapSize.x - 1 : 6;

            // Wall Data
            var defaltMaterialID = -1;
            dataOut.SavedWallDatas = new List<WallSaveData>();

            for (var x = 1; x <= dataOut.SavedMapSize.x; x++)
                dataOut.SavedWallDatas.Add(new WallSaveData(new Vector3Int(x, 0, 0), defaltMaterialID));

            for (var y = 1; y <= dataOut.SavedMapSize.y; y++)
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