using System;
using Disco_ScriptableObject;
using PropBehaviours;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class WallData
    {
        public Wall AssignedWall { get; private set; }
        public Vector3Int CellPosition { get; private set; }
        public int AssignedMaterialID { get; private set; }

        public WallData(Vector3Int cellPosition, int assignedMaterialID)
        {
            CellPosition = cellPosition;
            AssignedMaterialID = assignedMaterialID;
        }

        public void AssignReferance(Wall assignment)
        {
            AssignedWall = assignment;
        }

        public void AssignNewID(MaterialItemSo materialItemSo)
        {
            if (AssignedWall == null)
            {
                Debug.LogError("Wall Object Not Assigned");
                return;
            }

            AssignedWall.UpdateMaterial(materialItemSo);
            AssignedMaterialID = materialItemSo.ID;
        }
    }
}