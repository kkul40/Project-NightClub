using System;
using System.Collections.Generic;
using PropBehaviours;
using UnityEngine;

namespace Data
{
    [DisallowMultipleComponent]
    public class DiscoData : Singleton<DiscoData>
    {
        [SerializeField] private MapGeneratorSystem _mapGeneratorSystem;
        public PlacementDataHandler placementDataHandler { get; private set; }

        [Header("Data")] 
        public List<Vector3> FloorMap = new();
        public List<Wall> WallMap = new();
        public List<Prop> GetPropList => placementDataHandler.GetPropList;
        public MapGeneratorSystem MapGenerator => _mapGeneratorSystem;
        public Vector3 EnterencePosition => GridHandler.Instance.GetCellCenterWorld(MapGenerator.DoorPosition);

        private void Awake()
        {
            placementDataHandler = new PlacementDataHandler();
        }

        public List<Wall> GetWallMapPosList()
        {
            return WallMap;
        }

        public enum eDanceStyle
        {
            Default,
            Hiphop,
        }

    }
}