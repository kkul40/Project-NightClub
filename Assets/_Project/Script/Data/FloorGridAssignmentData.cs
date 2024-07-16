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
        public FloorTile assignedFloorTile{ get; private set; }
        public int assignedMaterialID{ get; private set; }
        public int assignedSurfaceID{ get; private set; }
        public int assignedObjectID{ get; private set; }

        public FloorGridAssignmentData(Vector3Int cellPosition)
        {
            CellPosition = cellPosition;
            assignedMaterialID = -1;
            assignedSurfaceID = -1;
            assignedObjectID = -1;
        }
        
        public void AssignReferance(FloorTile assignment, Vector3Int cellPosition)
        {
            assignedFloorTile = assignment;
            CellPosition = cellPosition;
        }

        public void AssignNewID(eFloorGridAssignmentType layerToAssign, int newID)
        {
            if (assignedFloorTile == null)
            {
                Debug.LogError("Floor Object Not Assigned");
                return;
            }
            
            if (newID == -1)
            {
                assignedFloorTile.UpdateMaterial(InitConfig.Instance.GetDefaultTileMaterial.Material);
                return;
            }
            
            switch (layerToAssign)
            {
                case eFloorGridAssignmentType.NULL:
                    Debug.LogError("Null Placement Data on : " + CellPosition);
                    break;
                case eFloorGridAssignmentType.Material:
                    if (assignedMaterialID == newID) return; // Already Assigned
                    
                    var foundMaterial = DiscoData.Instance.AllInGameItems.FirstOrDefault(x => x.ID == newID) as MaterialItemSo;
                    if (foundMaterial == null)
                    {
                        Debug.LogError(newID + " Could Not Found in Item List");
                        break;
                    }
                    assignedFloorTile.UpdateMaterial(foundMaterial.Material);
                    assignedMaterialID = newID;
                    break;
                case eFloorGridAssignmentType.Surface:
                    if (assignedSurfaceID == newID) return;

                    assignedSurfaceID = newID;
                    break;
                case eFloorGridAssignmentType.Object:
                    if (assignedObjectID == newID) return;

                    assignedObjectID = newID;
                    break;
            }
            
            Debug.Log("New ID Assigned To FloorGridData");
        }
    }

    public enum eFloorGridAssignmentType
    {
        NULL,
        Material,
        Surface,
        Object,
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