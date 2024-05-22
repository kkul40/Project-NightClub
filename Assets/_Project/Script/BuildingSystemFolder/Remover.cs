// using System;
// using Data;
// using ScriptableObjects;
// using UnityEngine;
//
// namespace BuildingSystemFolder
// {
//     public class Remover : IBuilder
//     {
//         private BuildingManager BuildingManager => BuildingManager.Instance;
//         private Material defaultMaterial;
//         private MeshRenderer selectedMeshRenderer;
//         private GameObject placedObject = null;
//
//         private Vector3Int lastCellPos = -Vector3Int.one;
//         
//         public void Setup(PlacableItemSo placableItemSo)
//         {
//             // Do Nothing...
//         }
//
//         public void BuildUpdate()
//         {
//             TryRemoving();
//         }
//
//         public void Exit()
//         {
//             if (selectedMeshRenderer != null)
//             {
//                 selectedMeshRenderer.material = defaultMaterial;
//                 selectedMeshRenderer = null;
//             }
//             
//             BuildingManager.Instance.StopBuilding();
//         }
//         
//         public void TryRemoving()
//         {
//             Vector3Int cellPos = BuildingManager.Instance.GetMouseCellPosition();
//
//             if (cellPos != lastCellPos)
//             {
//                 placedObject = GameData.Instance.PlacementHandler.GetPlacedObjectByCellPosition(cellPos);
//                 SetMaterial(placedObject);
//                 lastCellPos = cellPos;
//             }
//
//             if (InputSystem.Instance.LeftClickOnWorld)
//             {
//                 if (placedObject != null)
//                 {
//                     if (placedObject.transform.TryGetComponent(out IOccupieable occupieable))
//                     {
//                         if (occupieable.IsOccupied)
//                         {
//                             Debug.LogError("This Object Is Occupied Do Not Remove");
//                         }
//                         else
//                         {
//                             RemovePlacedObject(cellPos);
//                         }
//                     }
//                     else
//                     {
//                         RemovePlacedObject(cellPos);
//                     }
//                 }
//             }
//             
//             if (InputSystem.Instance.Esc)
//             {
//                 Exit();
//             }
//         }
//
//         protected virtual void SetMaterial(GameObject placedObject)
//         {
//             // Hiraeth Yardimlariyla
//             if (selectedMeshRenderer != null)
//             {
//                 selectedMeshRenderer.material = defaultMaterial;
//                 selectedMeshRenderer = null;
//             }
//
//             if (placedObject != null)
//             {
//                 selectedMeshRenderer = placedObject.GetComponent<MeshRenderer>() != null ? placedObject.GetComponent<MeshRenderer>() : placedObject.GetComponentInChildren<MeshRenderer>();
//                 defaultMaterial = selectedMeshRenderer.material;
//                 selectedMeshRenderer.material = BuildingManager.yellowMaterial;
//             }            
//         }
//
//         protected virtual void RemovePlacedObject(Vector3Int cellPos)
//         {
//             GameData.Instance.PlacementHandler.RemovePlacementData(cellPos);
//         }
//     }
// }

