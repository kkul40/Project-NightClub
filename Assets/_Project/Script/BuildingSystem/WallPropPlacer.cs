// using System;
// using Data;
// using ScriptableObjects;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace BuildingSystemFolder
// {
//     public class WallPropPlacer : IBuilder
//     {
//         private BuildingManager BuildingManager => BuildingManager.Instance;
//         private LayerMask placableLayer;
//         
//         private Vector3 placingOffset = new Vector3(0f,0,0f);
//         private Quaternion lastRotation = Quaternion.identity;
//
//         private WallPropSo _wallItemSo;
//         private GameObject tempPrefab;
//         private MeshRenderer tempMeshRenderer;
//         
//         private Vector3Int lastCellPos = -Vector3Int.one;
//         private bool isPlacable = false;
//         private bool lastPlacable = false;
//         private Vector3 nextPlacableGridPos = Vector3.zero;
//         private Vector3Int snappedCellPos = Vector3Int.zero;
//
//         public void Setup(PlacableItemSo placableItemSo)
//         {
//             // if (placableItemSo is WallItemSo placable)
//             // {
//             //     _wallItemSo = placable;
//             //     tempPrefab = Object.Instantiate(placable.Prefab, Vector3.zero, lastRotation);
//             //     tempMeshRenderer = tempPrefab.GetComponent<MeshRenderer>();
//             //     BuildingSystem.Instance.GetTileIndicator.SetTileIndicator(PlacingType.Place);
//             // }
//         }
//
//         public void BuildUpdate()
//         {
//             TryPlacing();
//         }
//
//         public void Exit()
//         {
//             Object.Destroy(tempPrefab);
//             BuildingManager.Instance.StopBuilding();
//         }
//       
//         public void TryPlacing()
//         {
//             Vector3Int cellPos = BuildingManager.GetMouseCellPosition();
//             Vector3 mousePos = InputSystem.Instance.GetMouseMapPosition();
//             Vector3Int offset = Vector3Int.up * cellPos.y;
//
//             isPlacable = !GameData.Instance.PlacementHandler.ContainsKey(snappedCellPos, _wallItemSo.ObjectSize, DirectionHelper.GetDirectionFromQuaternion(lastRotation));
//             
//             if (cellPos != lastCellPos || isPlacable != lastPlacable)
//             {
//                 nextPlacableGridPos = BuildingManager.GetCellCenterWorld(GetClosestWall(mousePos) + offset);
//                 snappedCellPos = BuildingManager.GetWorldToCell(nextPlacableGridPos);
//                 SetMaterialsColor(isPlacable);
//                 lastCellPos = cellPos;
//                 lastPlacable = isPlacable;
//             }
//             
//             tempPrefab.transform.position = Vector3.Lerp(tempPrefab.transform.position, nextPlacableGridPos, Time.deltaTime * BuildingManager.GetObjectPlacingSpeed());
//             tempPrefab.transform.rotation = lastRotation; // Smooth Rotation
//
//             if (InputSystem.Instance.LeftClickOnWorld)
//             {
//                 if (isPlacable)
//                 {
//                     tempPrefab.transform.position = nextPlacableGridPos;
//                     Place(snappedCellPos);
//                 }
//             }
//
//             if (InputSystem.Instance.Esc)
//             {
//                 Exit();
//             }
//         }
//
//         private Vector3Int GetClosestWall(Vector3 cellPos)
//         {
//             float lastDis = 9999;
//             Vector3 closestWall = Vector3.zero;
//             foreach(var pos in GameData.Instance.GetWallMapPosList())
//             {
//                 if (pos is WallDoor) continue;
//                 
//                 var dis = Vector3.Distance(cellPos, pos.transform.position);
//                 if (dis < lastDis)
//                 {
//                     closestWall = pos.transform.position;
//                     lastRotation = pos.transform.localRotation;
//                     lastDis = dis;
//                 }
//             }
//
//             return BuildingManager.GetWorldToCell(closestWall);
//         }
//         
//         private void Place(Vector3Int CellPosition)
//         {
//             // var newObject = Object.Instantiate(_wallItemSo.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
//             // newObject.transform.SetParent(_buildingSystem.GetSceneTransformContainer().PropHolderTransform);
//             //
//             // Direction currentDirection = DirectionHelper.GetDirectionFromQuaternion(lastRotation);
//             //
//             // if (newObject.TryGetComponent(out Prop prop))
//             // {
//             //     prop.Initialize(_wallItemSo, CellPosition, currentDirection);
//             // }
//             //
//             // GameData.Instance.PlacementHandler.AddPlacementData(CellPosition, new PlacementData(_wallItemSo, newObject, _wallItemSo.ObjectSize, currentDirection));
//         }
//         
//         private void SetMaterialsColor(bool isCellPosValid)
//         {
//             Material placementMaterial = isCellPosValid ? BuildingManager.blueMaterial : BuildingManager.redMaterial;
//             tempMeshRenderer.material = placementMaterial;
//         }
//     }
// }

