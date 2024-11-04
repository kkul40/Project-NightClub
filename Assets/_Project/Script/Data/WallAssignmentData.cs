using System;
using System.Linq;
using BuildingSystem.SO;
using PropBehaviours;
using Root;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class WallAssignmentData
    {
        public Vector3Int CellPosition = -Vector3Int.one;
        public Wall assignedWall { get; private set; }
        public int assignedMaterialID { get; private set; }

        public WallAssignmentData(Vector3Int cellPosition)
        {
            CellPosition = cellPosition;
            assignedMaterialID = -1;
        }

        public WallAssignmentData(GameDataExtension.WallSaveData wallSaveData)
        {
            CellPosition = wallSaveData.CellPosition;
            assignedMaterialID = wallSaveData.AssignedMaterialID;
        }

        public void AssignReferance(Wall assignment)
        {
            assignedWall = assignment;
        }

        public void AssignNewID(MaterialItemSo materialItemSo)
        {
            if (assignedWall == null)
            {
                Debug.LogError("Wall Object Not Assigned");
                return;
            }

            assignedWall.UpdateMaterial(materialItemSo);
            assignedMaterialID = materialItemSo.ID;
        }
    }
}