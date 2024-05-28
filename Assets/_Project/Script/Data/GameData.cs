using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [DisallowMultipleComponent]
    public class GameData : Singleton<GameData>
    {
        [SerializeField] private MapGeneratorSystem _mapGeneratorSystem;
        public PlacementDataHandler placementDataHandler { get; private set; }

        [Header("Data")] public List<Vector3> FloorMap = new();
        public List<Wall> WallMap = new();
        public List<Prop> GetPropList => placementDataHandler.GetPropList;
        public MapGeneratorSystem MapGenerator => _mapGeneratorSystem;

        private void Awake()
        {
            placementDataHandler = new PlacementDataHandler();
        }

        public List<Wall> GetWallMapPosList()
        {
            return WallMap;
        }
    }
}