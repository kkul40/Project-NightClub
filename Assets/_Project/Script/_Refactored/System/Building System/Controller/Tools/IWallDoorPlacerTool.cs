using Data;
using Disco_Building;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;

namespace System.Building_System.Controller.Tools
{
    public class IWallDoorPlacerTool : ITool
    {
        private GameObject _tempObject;
        private WallDoor _tempWallDoor;
        private AutoDoor _autoDoor;
        private int _tempMaterialID;
        private Quaternion _wallRotation;
        private bool isWallOnX;
        private bool updateGuard = false;

        private WallData _closestAssignmentData;
        private WallData _currentAssignmentData;

        private WallData _storedAssignmentData;
        private Vector3 _storedPosition;
        private bool _storedIsWallOnX;
        private Quaternion _storedRotation;
        private Vector3Int _storeedCellPosition;

        public bool isFinished { get; private set; }
        public void OnStart(ToolHelper TH)
        {
            _storedAssignmentData = TH.DiscoData.MapData.WallDatas.Find(x => x.assignedWall is WallDoor);
            _storeedCellPosition = _storedAssignmentData.CellPosition;
            _storedPosition = _storedAssignmentData.assignedWall.transform.position;
            _storedRotation = _storedAssignmentData.assignedWall.transform.rotation;
            _storedIsWallOnX = _storedRotation != RotationData.Left.rotation;
            
            var newWall = MapGeneratorSystem.Instance.CreateObject(MapGeneratorSystem.Instance.GetWallPrefab, _storedAssignmentData.assignedWall.transform.position, _storedAssignmentData.assignedWall.transform.rotation, false);
            newWall.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            TH.DiscoData.MapData.RemoveWallData(_storedAssignmentData.CellPosition);
            var newData = TH.DiscoData.MapData.AddNewWallData(_storedAssignmentData.CellPosition, newWall);
            newData.AssignNewID(TH.DiscoData.FindAItemByID(_storedAssignmentData.assignedMaterialID) as MaterialItemSo);
            
            _tempObject = MonoBehaviour.Instantiate(MapGeneratorSystem.Instance.GetWallDoorPrefab, _storeedCellPosition.FlattenToGround(), Quaternion.identity);
            _tempWallDoor = _tempObject.GetComponent<WallDoor>();
            _autoDoor = _tempObject.GetComponentInChildren<AutoDoor>();
            _autoDoor.Locked = true;
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (InputSystem.Instance.HasMouseMoveToNewCell) return false;

            Vector3 position = GetPlacementPosition(_closestAssignmentData.assignedWall.transform.position, isWallOnX);
            
            Vector3 enterancePosition = TH.DiscoData.MapData.EnterencePosition(position.WorldPosToCellPos(eGridType.PlacementGrid));
            Vector3 enteranceCellPos = enterancePosition.WorldPosToCellPos(eGridType.PlacementGrid).CellCenterPosition(eGridType.PlacementGrid);
            
            var colliders = Physics.OverlapBox(enteranceCellPos.Add(y:1),new Vector3(0.5f, 1, 0.5f) * ToolHelper.HitCollisionLeniency, Quaternion.identity);
            for (int i = 0; i < colliders.Length; i++)
            {
                var hitObject = colliders[i];
            
                var hitUnit = hitObject.GetComponentInParent<IPropUnit>();
                if (hitUnit == null || hitUnit.transform == _tempObject.transform)
                    continue;

                IPropUnit propUnit = hitObject.GetComponentInParent<IPropUnit>();
           
                if (propUnit != null)
                {
                    if (propUnit.PlacementLayer == ePlacementLayer.FloorProp || propUnit.PlacementLayer == ePlacementLayer.WallProp)
                        return false;
                }
            }
            return true;
        }

        public void OnUpdate(ToolHelper TH)
        {
            if (TH.InputSystem.HasMouseMoveToNewCell) return;
            
            UpdpateClosestWall(TH);
            
            _tempObject.transform.position = GetPlacementPosition(_closestAssignmentData.assignedWall.transform.position, isWallOnX);
            _tempObject.transform.rotation = _wallRotation;

            if (_closestAssignmentData.assignedMaterialID != _tempMaterialID)
            {
                _tempMaterialID = _closestAssignmentData.assignedMaterialID;
                _tempWallDoor.UpdateMaterial(TH.DiscoData.FindAItemByID(_tempMaterialID) as MaterialItemSo);
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
        }


        public void OnPlace(ToolHelper TH)
        {
            Vector3 position = GetPlacementPosition(_closestAssignmentData.assignedWall.transform.position, isWallOnX);
            
            var newWallDoorObject = MapGeneratorSystem.Instance.CreateObject(MapGeneratorSystem.Instance.GetWallDoorPrefab, position, _wallRotation, false);
            newWallDoorObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            TH.DiscoData.MapData.ChangeDoorPosition((int)Mathf.Max(position.x, position.z) + 1, isWallOnX);
            
            MonoBehaviour.Destroy(_closestAssignmentData.assignedWall.gameObject);
            
            _closestAssignmentData.AssignReferance(newWallDoorObject.GetComponent<Wall>());
            
            var found = DiscoData.Instance.FindAItemByID(_closestAssignmentData.assignedMaterialID) as MaterialItemSo;
            if (found == null)
                _closestAssignmentData.AssignNewID(InitConfig.Instance.GetDefaultWallMaterial);
            else
                _closestAssignmentData.AssignNewID(found);

            TH.FXCreatorSystem.CreateFX(FXType.Wall, position, Vector2.one, _wallRotation);

            isFinished = true;
        }

        public void OnStop(ToolHelper TH)
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
            TH.DiscoData.MapData.RemoveWallData(_storeedCellPosition);
            var newData = TH.DiscoData.MapData.AddNewWallData(_storeedCellPosition, newWallDoorObject);
            newData.AssignNewID(TH.DiscoData.FindAItemByID(_storedAssignmentData.assignedMaterialID) as MaterialItemSo);
        }

        public bool CheckPlaceInput(ToolHelper TH)
        {
            return TH.InputSystem.LeftClickOnWorld;
        }

        private void UpdpateClosestWall(ToolHelper TH)
        {
            float lastDis = 9999;
            Vector3 closestWallPos = Vector3.zero;
            foreach (var wall in TH.DiscoData.MapData.WallDatas)
            {
                var dis = Vector3.Distance(TH.InputSystem.MousePosition, wall.assignedWall.transform.position);
                
                if (dis < lastDis)
                {
                    _closestAssignmentData = wall;
                    _wallRotation = wall.assignedWall.transform.rotation;
                    isWallOnX = _wallRotation.GetDirectionFromQuaternion() != Direction.Left;
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