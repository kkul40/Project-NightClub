using System;
using BuildingSystem.SO;
using Data;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildingNeedsData
    {
        public InputSystem InputSystem;
        public GameData GameData;
        public SceneTransformContainer SceneTransformContainer;
        public MaterialColorChanger MaterialColorChanger;
        
        public StoreItemSO StoreItemSo;
        public Vector3Int CellPosition;
        public Vector3 CellCenterPosition;
        public RotationData RotationData;
        public float MoveSpeed = 10;

        public BuildingNeedsData(InputSystem inputSystem, GameData gameData, SceneTransformContainer sceneTransformContainer, MaterialColorChanger materialColorChanger)
        {
            InputSystem = inputSystem;
            GameData = gameData;
            SceneTransformContainer = sceneTransformContainer;
            MaterialColorChanger = materialColorChanger;
            RotationData = new RotationData();
        }
    }
}