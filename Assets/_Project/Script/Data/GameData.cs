using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [DisallowMultipleComponent]
    public class GameData : Singleton<GameData>
    {
        [SerializeField] private MapGeneratorSystem _mapGeneratorSystem;

        [Header("Data")] public List<Vector3> FloorMap = new();

        public List<Wall> WallMap = new();
        [SerializeField] private PlacamentDataHandler _placamentDataHandler;
        public List<Prop> GetPropList => PlacementHandler.GetPropList;
        public PlacamentDataHandler PlacementHandler => _placamentDataHandler;
        public MapGeneratorSystem MapGenerator => _mapGeneratorSystem;

        private void Awake()
        {
            _placamentDataHandler = new PlacamentDataHandler();
        }

        public List<Wall> GetWallMapPosList()
        {
            return WallMap;
        }
    }
}