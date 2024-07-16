using System;
using BuildingSystem;
using BuildingSystem.SO;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class PlacementData
    {
        //TODO Optime It
        public int ID;
        public Vector3Int PlacedCellPos;
        public GameObject SceneObject;
        public StoreItemSO storeItemSo;
        public Vector2Int Size;
        public RotationData RotationData;
        public ePlacementLayer PlacementLayer;
        public eMaterialLayer MaterialLayer;

        public PlacementData()
        {
            ID = -1;
        }

        public PlacementData(PlacementItemSO storeItemSo, Vector3Int placedCellPos, GameObject createdObject, Vector2Int Size, RotationData rotationData)
        {
            ID = storeItemSo.ID;
            PlacedCellPos = placedCellPos;
            this.storeItemSo = storeItemSo as PlacementItemSO;
            SceneObject = createdObject;
            RotationData = rotationData;
            this.Size = Size;
            PlacementLayer = storeItemSo.PlacementLayer;
            MaterialLayer = eMaterialLayer.Null;
        }
        
        public PlacementData(MaterialItemSo storeItemSo)
        {
            ID = Guid.NewGuid().GetHashCode();
            this.storeItemSo = storeItemSo as MaterialItemSo;
            SceneObject = null;
            RotationData = BuildingSystem.RotationData.Default;
            this.Size = Vector2Int.zero;
            MaterialLayer = storeItemSo.MaterialLayer;
            PlacementLayer = ePlacementLayer.Null;
        }
      
    }
}