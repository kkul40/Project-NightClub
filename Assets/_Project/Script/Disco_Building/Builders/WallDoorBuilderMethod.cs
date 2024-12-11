using System;
using Data;
using Disco_ScriptableObject;
using DiscoSystem;
using ExtensionMethods;
using PropBehaviours;
using Unity.Mathematics;
using UnityEngine;

namespace Disco_Building.Builders
{
    public class WallDoorBuilderMethod : IBuildingMethod
    {
        public bool PressAndHold { get; } = false;
        public bool isFinished { get; private set; }

        private GameObject _tempObject;
        private WallDoor _tempWallDoor;
        private AutoDoor _autoDoor;
        private int _tempMaterialID;
        private quaternion _wallRotation;
        private bool isWallOnX;
        private bool updateGuard = false;

        private WallAssignmentData _closestAssignmentData;
        private WallAssignmentData _currentAssignmentData;

        private WallAssignmentData _storedAssignmentData;
        private Vector3 _storedPosition;
        private bool _storedIsWallOnX;
        private quaternion _storedRotation;
        private Vector3Int _storeedCellPosition;

        public void OnStart(BuildingNeedsData BD)
        {
            /*
             * Add Wall to Door Position
             * Remove Door From Data
             * Create New Door
             */
            

            // Store Values
            _storedAssignmentData = BD.DiscoData.MapData.WallDatas.Find(x => x.assignedWall is WallDoor);
            _storeedCellPosition = _storedAssignmentData.CellPosition;
            _storedPosition = _storedAssignmentData.assignedWall.transform.position;
            _storedRotation = _storedAssignmentData.assignedWall.transform.rotation;
            _storedIsWallOnX = _storedRotation != RotationData.Left.rotation;
            //
            
            var newWall = MapGeneratorSystem.Instance.CreateObject(MapGeneratorSystem.Instance.GetWallPrefab, _storedAssignmentData.assignedWall.transform.position, _storedAssignmentData.assignedWall.transform.rotation, false);
            newWall.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            BD.DiscoData.MapData.RemoveWallData(_storedAssignmentData.CellPosition);
            var newData = BD.DiscoData.MapData.AddNewWallData(_storedAssignmentData.CellPosition, newWall);
            newData.AssignNewID(BD.DiscoData.FindAItemByID(_storedAssignmentData.assignedMaterialID) as MaterialItemSo);
            
            _tempObject = MonoBehaviour.Instantiate(MapGeneratorSystem.Instance.GetWallDoorPrefab, BD.CellPosition.FlattenToGround(), Quaternion.identity);
            _tempWallDoor = _tempObject.GetComponent<WallDoor>();
            _autoDoor = _tempObject.GetComponentInChildren<AutoDoor>();
            _autoDoor.Locked = true;
        }

        public bool OnValidate(BuildingNeedsData BD)
        {
            if (!BD.IsCellPosInBounds()) return false;

            if (InputSystem.Instance.HasMouseMoveToNewCell) return false;

            Vector3 position = GetPlacementPosition(_closestAssignmentData.assignedWall.transform.position, isWallOnX);
            
            Vector3 enterancePosition = BD.DiscoData.MapData.EnterencePosition(position.WorldPosToCellPos(eGridType.PlacementGrid));
            Vector3Int enteranceCellPos = enterancePosition.WorldPosToCellPos(eGridType.PlacementGrid);
            
            if(BD.DiscoData.placementDataHandler.ContainsKey(enteranceCellPos, ePlacementLayer.FloorProp))
                return false;

            for (int i = 0; i < ConstantVariables.DoorHeight; i++)
            {
                if(BD.DiscoData.placementDataHandler.ContainsKey(enteranceCellPos.Add(y:i), ePlacementLayer.WallProp))
                    return false;
            }
            
            return true;
        }

        public void OnUpdate(BuildingNeedsData BD)
        {
            if (!BD.IsCellPosInBounds() && updateGuard) return;
            
            UpdpateClosestWall(BD);
            
            // _tempObject.transform.position = Vector3.Lerp(_tempObject.transform.position,
            //     GetPlacementPosition(_closestAssignmentData.assignedWall.transform.position, isWallOnX) + new Vector3(0.02f, 0.02f, 0.02f),
            //     Time.deltaTime * BD.MoveSpeed);

            
            _tempObject.transform.position = GetPlacementPosition(_closestAssignmentData.assignedWall.transform.position, isWallOnX);
            _tempObject.transform.rotation = _wallRotation;

            if (_closestAssignmentData.assignedMaterialID != _tempMaterialID)
            {
                _tempMaterialID = _closestAssignmentData.assignedMaterialID;
                _tempWallDoor.UpdateMaterial(BD.DiscoData.FindAItemByID(_tempMaterialID) as MaterialItemSo);
            }

            if (_currentAssignmentData != _closestAssignmentData)
            {
                if (_currentAssignmentData != null)
                {
                    _currentAssignmentData.assignedWall.gameObject.SetActive(true);
                }
                
                _currentAssignmentData = _closestAssignmentData;
                _currentAssignmentData.assignedWall.gameObject.SetActive(false);
            }

            updateGuard = true;
        }

        public void OnPlace(BuildingNeedsData BD)
        {
            Vector3 position = GetPlacementPosition(_closestAssignmentData.assignedWall.transform.position, isWallOnX);
            
            var newWallDoorObject = MapGeneratorSystem.Instance.CreateObject(MapGeneratorSystem.Instance.GetWallDoorPrefab, position, _wallRotation, false);
            newWallDoorObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            BD.DiscoData.MapData.ChangeDoorPosition((int)Mathf.Max(position.x, position.z) + 1, isWallOnX);
            
            MonoBehaviour.Destroy(_closestAssignmentData.assignedWall.gameObject);
            
            _closestAssignmentData.AssignReferance(newWallDoorObject.GetComponent<Wall>());
            
            var found = DiscoData.Instance.FindAItemByID(_closestAssignmentData.assignedMaterialID) as MaterialItemSo;
            if (found == null)
                _closestAssignmentData.AssignNewID(InitConfig.Instance.GetDefaultWallMaterial);
            else
                _closestAssignmentData.AssignNewID(found);

            
            isFinished = true;
        }

        public void OnStop(BuildingNeedsData BD)
        {
            if(_tempObject != null) 
                MonoBehaviour.Destroy(_tempObject);

            if (_currentAssignmentData != null)
            {
                _currentAssignmentData.assignedWall.gameObject.SetActive(true);
            }
            
            if (isFinished) return;
            
            Vector3 position = GetPlacementPosition(_storedPosition, _storedIsWallOnX);
            var newWallDoorObject = MapGeneratorSystem.Instance.CreateObject(MapGeneratorSystem.Instance.GetWallDoorPrefab, position, _storedRotation, false);
            newWallDoorObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);

            // var data = BD.DiscoData.MapData.GetWallDataByCellPos(_storeedCellPosition);
            BD.DiscoData.MapData.RemoveWallData(_storeedCellPosition);
            var newData = BD.DiscoData.MapData.AddNewWallData(_storeedCellPosition, newWallDoorObject);
            newData.AssignNewID(BD.DiscoData.FindAItemByID(_storedAssignmentData.assignedMaterialID) as MaterialItemSo);
        }
        
        private void UpdpateClosestWall(BuildingNeedsData BD)
        {
            float lastDis = 9999;
            Vector3 closestWallPos = Vector3.zero;
            foreach (var wall in BD.DiscoData.MapData.WallDatas)
            {
                var dis = Vector3.Distance(BD.InputSystem.MousePosition, wall.assignedWall.transform.position);
                
                if (dis < lastDis)
                {
                    _closestAssignmentData = wall;
                    _wallRotation = wall.assignedWall.transform.rotation;
                    isWallOnX = _wallRotation != RotationData.Left.rotation;
                    lastDis = dis;
                }
            }
        }

        private Vector3 GetPlacementPosition(Vector3 position, bool isWallOnX)
        {
            Vector3Int cellPosition = position.WorldPosToCellPos(eGridType.PlacementGrid);
            return isWallOnX ? new Vector3(cellPosition.x + 0.5f, 0, 0) : new Vector3(0, 0, cellPosition.z + 0.5f);
        }
    }
}