using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class NonePlacementMethod : IPlacementMethod
    {
        public bool pressAndHold { get; } = false;
        public Vector3 offset { get; }

        public bool CanPlace(Vector3Int cellPos, IPlaceableItemData placeableItemData, RotationData rotationData)
        {
            Debug.LogError("None Placement");
            return false;
        }

        public void LogicUpdate(Vector3Int cellPos, IPlaceableItemData placeableItemData, RotationData rotationData)
        {
            Debug.LogError("None Placement");
        }

        public GameObject Place(Vector3Int cellPosInt, Vector3 cellPos, IPlaceableItemData placeableItemData,
            RotationData rotationData)
        {
            Debug.LogError("None Placement");
            return null;
        }
    }
}