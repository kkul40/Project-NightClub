using System;
using Data;
using ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BuildingSystemFolder
{
    public class DanceFloorPlacementMethod : IPlacementMethod
    {
        private readonly int FloorLayerID = 7;
        public bool pressAndHold { get; } = true;
        public Vector3 offset { get; } = new(0, -0.5f, 0);

        public bool CanPlace(Vector3Int cellPos, IPlaceableItemData placeableItemData, RotationData rotationData)
        {
            var transform = InputSystem.Instance.GetHitTransformWithLayer(FloorLayerID);
            if (transform == null) return false;

            if (GameData.Instance.PlacementHandler.ContainsKey(cellPos, placeableItemData.Size, rotationData,
                    PlacementLayer.FloorLevel)) return false;
            return true;
        }

        public void LogicUpdate(Vector3Int cellPos, IPlaceableItemData placeableItemData, RotationData rotationData)
        {
        }

        public GameObject Place(Vector3Int cellPosInt, Vector3 cellPos, IPlaceableItemData placeableItemData,
            RotationData rotationData)
        {
            var newObject = Object.Instantiate(placeableItemData.Prefab, cellPos, rotationData.rotation);
            newObject.transform.SetParent(BuildingManager.Instance.GetSceneTransformContainer.PropHolderTransform);
            GameData.Instance.PlacementHandler.AddPlacementData(cellPosInt,
                new PlacementData(placeableItemData, newObject, rotationData), PlacementLayer.FloorLevel);

            return newObject;
        }
    }
}