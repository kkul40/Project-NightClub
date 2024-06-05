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
        public SceneGameObjectHandler SceneGameObjectHandler;
        public MaterialColorChanger MaterialColorChanger;
        
        public StoreItemSO StoreItemSo;
        public Vector3Int CellPosition;
        public Vector3 CellCenterPosition;
        public RotationData RotationData;
        public float MoveSpeed = 10;

        public BuildingNeedsData(InputSystem inputSystem, DiscoData discoData, SceneGameObjectHandler sceneGameObjectHandler, MaterialColorChanger materialColorChanger)
        {
            InputSystem = inputSystem;
            DiscoData = discoData;
            SceneGameObjectHandler = sceneGameObjectHandler;
            MaterialColorChanger = materialColorChanger;
            RotationData = RotationData.Default;
        }
    }
}