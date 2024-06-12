using System;
using System.Collections.Generic;
using PropBehaviours;
using UnityEngine;

namespace Data
{
    [DisallowMultipleComponent]
    public class DiscoData : Singleton<DiscoData>
    {
        public PlacementDataHandler placementDataHandler { get; private set; }
        public MapData mapData;
        
        public List<IPropUnit> GetPropList => placementDataHandler.GetPropList;

        // FIRST INIT IN THE GAME
        private void Awake()
        {
            mapData = new MapData();
            placementDataHandler = new PlacementDataHandler();
        }

        [Serializable]
        public class MapData
        {
            public static readonly int MaxMapSizeX = 50;
            public static readonly int MaxMapSizeY = 50;
            public static readonly Vector2Int InitialMapSize = new Vector2Int(11, 11);
            public Vector2Int CurrentMapSize = Vector2Int.zero;
            
            // TODO Make door ps vector2Int
            public Vector3Int DoorPosition = Vector3Int.zero;
            
            public TileNode[,] TileNodes { get; private set; }
            public List<Wall> WallMap;
            public List<Vector3> FloorMap;

            public MapData()
            {
                TileNodes = new TileNode[MaxMapSizeX, MaxMapSizeY];

                for (int i = 0; i < MaxMapSizeX; i++)
                for (int j = 0; j < MaxMapSizeY; j++)
                    TileNodes[i, j] = new TileNode(false, -Vector3.one, -1, -1);
                
                WallMap = new List<Wall>();
                FloorMap = new List<Vector3>();
            }

            public bool SetCurrentMapSize(MapGeneratorSystem mapGeneratorSystem, Vector2Int mapSize)
            {
                CurrentMapSize = mapSize;
                return true;
            }
            
            public List<Wall> GetWallMapPosList()
            {
                return WallMap;
            }

            public TileNode SetTileNodeByCellPos(Vector3Int cellpos)
            {
                if (cellpos.x > CurrentMapSize.x || cellpos.z > CurrentMapSize.y)
                {
                    Debug.LogError("TileNode Index Is Not Valid");
                    return null;
                }
                return TileNodes[cellpos.x, cellpos.z];
            }
            
            public Vector3 EnterencePosition => GridHandler.Instance.GetCellCenterWorld(DoorPosition);
        }
       
        public enum eDanceStyle
        {
            Default,
            Hiphop,
        }
    }
}