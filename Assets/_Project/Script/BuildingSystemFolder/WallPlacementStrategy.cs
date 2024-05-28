using System;
using Data;
using ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BuildingSystemFolder
{
    public class WallPlacementMethod : IPlacementMethod
    {
        private readonly int WallLayerID = 8;

        public bool pressAndHold { get; } = false;
        public Vector3 offset { get; }

        public bool CanPlace(Vector3Int cellPos, IPlaceableItemData placeableItemData, RotationData rotationData)
        {
            var transform = InputSystem.Instance.GetHitTransformWithLayer(WallLayerID);
            if (transform == null) return false;

            if (GameData.Instance.placementDataHandler.ContainsKey(cellPos, placeableItemData.Size, rotationData,
                    PlacementLayer.Surface)) return false;
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
            GameData.Instance.placementDataHandler.AddPlacementData(cellPosInt,
                new PlacementData(placeableItemData, newObject, rotationData), PlacementLayer.Surface);

            return newObject;
        }
    }
}