using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public struct PlacementData
    {
        public int ID;
        public GameObject SceneObject;
        public IPlaceableItemData placableItemData;
        public RotationData RotationData;

        public PlacementData(IPlaceableItemData placableItemData, GameObject sceneObject, RotationData rotationData)
        {
            ID = Guid.NewGuid().GetHashCode();
            this.placableItemData = placableItemData;
            SceneObject = sceneObject;
            RotationData = rotationData;
        }
    }
}