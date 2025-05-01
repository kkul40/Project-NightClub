using System;
using Data.New;
using Disco_ScriptableObject;
using PropBehaviours;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class FloorData
    {
        public Vector3Int CellPosition { get; private set; }
        public FloorTile assignedFloorTile { get; private set; }
        public int assignedMaterialID { get; private set; }

        public FloorData(Vector3Int cellPosition)
        {
            CellPosition = cellPosition;
            assignedMaterialID = InitConfig.Instance.GetDefaultTileMaterial.ID;
        }

        public FloorData(Save_MapData.Save_FloorData saveFloorData)
        {
            CellPosition = saveFloorData.cellPosition;
            assignedMaterialID = saveFloorData.assignedMaterialID;
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