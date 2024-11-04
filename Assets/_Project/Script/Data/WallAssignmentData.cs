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

        public void AssignNewID(int newID)
        {
            if (assignedWall == null)
            {
                Debug.LogError("Wall Object Not Assigned");
                return;
            }

            if (newID == -1)
            {
                assignedWall.UpdateMaterial(InitConfig.Instance.GetDefaultWallMaterial.Material);
                return;
            }

            var foundMaterial = DiscoData.Instance.FindAItemByID(newID) as MaterialItemSo;
            if (foundMaterial == null)
            {
                Debug.LogError(newID + " Could Not Found in Item List");
                return;
            }

            assignedWall.UpdateMaterial(foundMaterial.Material);
            assignedMaterialID = newID;

            KDebug.Print("New ID Assigned to Wall : " + newID);
        }
    }
}