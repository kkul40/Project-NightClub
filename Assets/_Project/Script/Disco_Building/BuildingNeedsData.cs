using System;
using Data;
using Disco_ScriptableObject;
using DiscoSystem;
using PropBehaviours;
using UnityEngine;

namespace Disco_Building
{
    public class BuildingNeedsData
    {
        public InputSystem InputSystem;
        public DiscoData DiscoData;
        public MaterialColorChanger MaterialColorChanger;
        public FXCreatorSystem FXCreatorSystem;

        public StoreItemSO StoreItemSo;
        public Vector3Int CellPosition;
        public Vector3 CellCenterPosition;
        public RotationData RotationData;
        public float MoveSpeed = 10;
        public bool isReplacing = false;

        public WallDoor WallDoor = null;

        public BuildingNeedsData(InputSystem inputSystem, DiscoData discoData,
            MaterialColorChanger materialColorChanger, FXCreatorSystem fxCreatorSystem)
        {
            InputSystem = inputSystem;
            DiscoData = discoData;
            MaterialColorChanger = materialColorChanger;
            FXCreatorSystem = fxCreatorSystem;
            RotationData = RotationData.Default;
        }

        public bool IsCellPosInBounds()
        {
            if (CellPosition.x >= DiscoData.MapData.CurrentMapSize.x ||
                CellPosition.z >= DiscoData.MapData.CurrentMapSize.y) return false;

            if (CellPosition.x < 0 || CellPosition.z < 0) return false;

            return true;
        }
    }
}