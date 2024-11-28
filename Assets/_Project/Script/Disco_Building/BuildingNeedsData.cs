using System;
using Data;
using Disco_ScriptableObject;
using PropBehaviours;
using UnityEngine;

namespace Disco_Building
{
    public class BuildingNeedsData
    {
        public InputSystem InputSystem;
        public DiscoData DiscoData;
        public MaterialColorChanger MaterialColorChanger;
        public FXCreator FXCreator;

        public StoreItemSO StoreItemSo;
        public Vector3Int CellPosition;
        public Vector3 CellCenterPosition;
        public RotationData RotationData;
        public float MoveSpeed = 10;
        public bool isReplacing = false;

        public WallDoor WallDoor = null;

        public BuildingNeedsData(InputSystem inputSystem, DiscoData discoData,
            MaterialColorChanger materialColorChanger, FXCreator fxCreator)
        {
            InputSystem = inputSystem;
            DiscoData = discoData;
            MaterialColorChanger = materialColorChanger;
            FXCreator = fxCreator;
            RotationData = RotationData.Default;
        }

        public bool IsCellPosInBounds()
        {
            if (CellPosition.x >= DiscoData.MapData.CurrentMapSize.x ||
                CellPosition.z >= DiscoData.MapData.CurrentMapSize.y) return false;

            if (CellPosition.x < 0 || CellPosition.y < 0) return false;

            return true;
        }
    }
}