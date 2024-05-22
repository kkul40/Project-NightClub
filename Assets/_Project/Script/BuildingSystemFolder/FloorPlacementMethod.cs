using System;
using Data;
using ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BuildingSystemFolder
{
    public class FloorPlacementMethod : IPlacementMethod
    {
        private readonly int FloorLayerID = 7;

        public bool pressAndHold { get; } = false;
        public Vector3 offset { get; } = new(0f, -0.5f, 0f);

        public bool CanPlace(Vector3Int cellPos, IPlaceableItemData placeableItemData, RotationData rotationData)
        {
            var hit = InputSystem.Instance.GetHitTransformWithLayer(FloorLayerID);

            if (hit == null) return false;

            if (GameData.Instance.PlacementHandler.ContainsKey(cellPos, placeableItemData.Size, rotationData, PlacementLayer.Surface)) return false;
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
                new PlacementData(placeableItemData, newObject, rotationData), PlacementLayer.Surface);

            return newObject;
        }
    }
}