using System;
using Data.New;
using Disco_ScriptableObject;
using PropBehaviours;
using SaveAndLoad;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class WallData
    {
        public Vector3Int CellPosition = -Vector3Int.one;
        public Wall assignedWall { get; private set; }
        public int assignedMaterialID { get; private set; }

        public WallData(Vector3Int cellPosition)
        {
            CellPosition = cellPosition;
            assignedMaterialID = -1;
        }

        public WallData(SavableMapData.WallSaveData wallSaveData)
        {
            CellPosition = wallSaveData.cellPosition;
            assignedMaterialID = wallSaveData.assignedMaterialID;
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