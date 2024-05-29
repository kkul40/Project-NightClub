using System;
using BuildingSystem;
using BuildingSystem.SO;
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
        public StoreItemSO storeItemSo;
        public Vector2Int Size;
        public RotationData RotationData;

        public PlacementData(StoreItemSO storeItemSo, GameObject createdObject, Vector2Int Size, RotationData rotationData)
        {
            ID = Guid.NewGuid().GetHashCode();
            this.storeItemSo = storeItemSo as PlacementItemSO;
            SceneObject = createdObject;
            RotationData = rotationData;
            this.Size = Size;
        }
      
    }
}