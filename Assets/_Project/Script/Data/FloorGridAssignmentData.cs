using System;
using System.Linq;
using BuildingSystem.SO;
using PropBehaviours;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class FloorGridAssignmentData
    {
        public Vector3Int CellPosition { get; private set; }
        public FloorTile assignedFloorTile { get; private set; }
        public int assignedMaterialID { get; private set; }

        public FloorGridAssignmentData(Vector3Int cellPosition)
        {
            CellPosition = cellPosition;
            assignedMaterialID = InitConfig.Instance.GetDefaultTileMaterial.ID;
        }

        public FloorGridAssignmentData(GameDataExtension.FloorSaveData floorSaveData)
        {
            CellPosition = floorSaveData.CellPosition;
            assignedMaterialID = floorSaveData.assignedMaterialID;
        }

        public void AssignReferance(FloorTile assignment, Vector3Int cellPosition)
        {
            assignedFloorTile = assignment;
            CellPosition = cellPosition;
        }

        public void AssignNewID(MaterialItemSo materialItemSo)
        {
            // TODO Instead of finding material here find it before you place and assign here
            if (assignedFloorTile == null)
            {
                Debug.LogError("Floor Object Not Assigned");
                return;
            }

            assignedFloorTile.UpdateMaterial(materialItemSo);
            assignedMaterialID = materialItemSo.ID;
        }
    }


    // Object idleri hashmap te tut gerektiginde sil


    /*
     * Cell Positionu bul
     * Ona atali olan Griddatayi bul
     * Ve degisikliklerini yap
     * Objeyi spawnla
     * objeyi hashmape kaydet
     */
}