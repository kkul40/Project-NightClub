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
            assignedMaterialID = -1;
        }

        public FloorGridAssignmentData(GameData.FloorSaveData floorSaveData)
        {
            CellPosition = floorSaveData.CellPosition;
            assignedMaterialID = floorSaveData.assignedMaterialID;
        }

        public void AssignReferance(FloorTile assignment, Vector3Int cellPosition)
        {
            assignedFloorTile = assignment;
            CellPosition = cellPosition;
        }

        public void AssignNewID(int newID)
        {
            if (assignedFloorTile == null)
            {
                Debug.LogError("Floor Object Not Assigned");
                return;
            }
            
            if (newID == -1) assignedFloorTile.UpdateMaterial(InitConfig.Instance.GetDefaultTileMaterial.Material);
            else
            {
                var foundMaterial = DiscoData.Instance.AllInGameItems.FirstOrDefault(x => x.ID == newID) as MaterialItemSo;
                if (foundMaterial == null)
                {
                    Debug.LogError(newID + " Could Not Found in Item List");
                }
                assignedFloorTile.UpdateMaterial(foundMaterial.Material);
            }

            assignedMaterialID = newID;
            
            Debug.Log("New ID Assigned To FloorGridData : " + newID);
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