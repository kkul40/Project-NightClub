using System;
using BuildingSystem.SO;
using Data;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildingNeedsData
    {
        public InputSystem InputSystem;
        public DiscoData DiscoData;
        public MaterialColorChanger MaterialColorChanger;

        public StoreItemSO StoreItemSo;
        public Vector3Int CellPosition;
        public Vector3 CellCenterPosition;
        public RotationData RotationData;
        public float MoveSpeed = 10;

        public BuildingNeedsData(InputSystem inputSystem, DiscoData discoData,
            MaterialColorChanger materialColorChanger)
        {
            InputSystem = inputSystem;
            DiscoData = discoData;
            MaterialColorChanger = materialColorChanger;
            RotationData = RotationData.Default;
        }

        public bool IsCellPosInBounds()
        {
            if (CellPosition.x >= DiscoData.MapData.CurrentMapSize.x ||
                CellPosition.z >= DiscoData.MapData.CurrentMapSize.y) return true;

            return false;
        }
    }
}