using _Initializer;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;

namespace DiscoSystem.Building_System.Controller.Tools
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
        private Vector3Int _storedCellPosition;

        private int _originalMaterialID;
        private int _originalWallIndex;

        public bool isFinished { get; private set; }
        public void OnStart(ToolHelper TH)
        {
            _storedAssignmentData = TH.DiscoData.MapData.GetWallDoor();
            _storedCellPosition = _storedAssignmentData.CellPosition;
            _storedPosition = _storedAssignmentData.AssignedWall.transform.position;
            _storedRotation = _storedAssignmentData.AssignedWall.transform.rotation;
            _storedIsWallOnX = _storedRotation != RotationData.Left.rotation;
            _originalMaterialID = _storedAssignmentData.AssignedMaterialID;
            _originalWallIndex = Mathf.Max(_storedCellPosition.x, _storedCellPosition.z); 
            
            var newWall = ServiceLocator.Get<MapGeneratorSystem>().CreateObject(ServiceLocator.Get<MapGeneratorSystem>().GetWallPrefab, _storedAssignmentData.AssignedWall.transform.position, _storedAssignmentData.AssignedWall.transform.rotation, false);
            newWall.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetWallHolder);

            
            TH.DiscoData.MapData.RemoveWallData(_storedAssignmentData.CellPosition);
            var newData = TH.DiscoData.MapData.AddNewWallData(_storedAssignmentData.CellPosition, newWall);
            
            
            newData.AssignNewID(GameBundle.Instance.FindAItemByID(_storedAssignmentData.AssignedMaterialID) as MaterialItemSo);
            
            _tempObject = Object.Instantiate(ServiceLocator.Get<MapGeneratorSystem>().GetWallDoorPrefab, _storedCellPosition.FlattenToGround(), Quaternion.identity);
            _tempWallDoor = _tempObject.GetComponent<WallDoor>();
            _autoDoor = _tempObject.GetComponentInChildren<AutoDoor>();
            _autoDoor.Locked = true;
            
            TH.TileIndicator.SetTileIndicator(ePlacingType.Direction, Vector2.one);
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (ServiceLocator.Get<InputSystem>().HasMouseMoveToNewCell) return false;

            Vector3 position = GetPlacementPosition(_closestAssignmentData.AssignedWall.transform.position, isWallOnX);
            
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
            
            UpdateClosestWall(TH);
            
            _tempObject.transform.position = GetPlacementPosition(_closestAssignmentData.AssignedWall.transform.position, isWallOnX);
            _tempObject.transform.rotation = _wallRotation;

            if (_closestAssignmentData.AssignedMaterialID != _tempMaterialID)
            {
                _tempMaterialID = _closestAssignmentData.AssignedMaterialID;
                _tempWallDoor.UpdateMaterial(GameBundle.Instance.FindAItemByID(_tempMaterialID) as MaterialItemSo);
            }

            if (_currentAssignmentData != _closestAssignmentData)
            {
                if (_currentAssignmentData != null)
                {
                    _currentAssignmentData.AssignedWall.gameObject.Activate();
                }
                
                _currentAssignmentData = _closestAssignmentData;
                _currentAssignmentData.AssignedWall.gameObject.InActivate();
                
                TH.TileIndicator.SetPositionAndRotation(_tempObject.transform.position + _tempObject.transform.forward * 0.5f, _wallRotation);
            }
        }


        public void OnPlace(ToolHelper TH)
        {
            Vector3 position = GetPlacementPosition(_closestAssignmentData.AssignedWall.transform.position, isWallOnX);
            
            var newWallDoorObject = ServiceLocator.Get<MapGeneratorSystem>().CreateObject(ServiceLocator.Get<MapGeneratorSystem>().GetWallDoorPrefab, position, _wallRotation, false);
            newWallDoorObject.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetWallHolder);

            int wallIndex = (int)Mathf.Max(position.x, position.z) + 1;
            TH.DiscoData.MapData.ChangeDoorPosition(wallIndex, isWallOnX);
            
            MonoBehaviour.Destroy(_closestAssignmentData.AssignedWall.gameObject);
            
            _closestAssignmentData.AssignReferance(newWallDoorObject.GetComponent<Wall>());
            
            var found = GameBundle.Instance.FindAItemByID(_closestAssignmentData.AssignedMaterialID) as MaterialItemSo;
            if (found == null)
                _closestAssignmentData.AssignNewID(ServiceLocator.Get<InitConfig>().GetDefaultWallMaterial);
            else
                _closestAssignmentData.AssignNewID(found);

            TH.FXCreatorSystem.CreateFX(FXType.Wall, position, Vector2.one, _wallRotation);
            TH.PlacementTracker.AddTrack(new WallDoorUndo(_storedIsWallOnX, _originalWallIndex, _originalMaterialID));
            
            isFinished = true;
        }

        public void OnStop(ToolHelper TH)
        {
            if(_tempObject != null) 
                MonoBehaviour.Destroy(_tempObject);

            if (_currentAssignmentData != null)
            {
                _currentAssignmentData.AssignedWall.gameObject.Activate();
            }
            
            if (isFinished) return;
            
            Vector3 position = GetPlacementPosition(_storedPosition, _storedIsWallOnX);
            var newWallDoorObject = ServiceLocator.Get<MapGeneratorSystem>().CreateObject(ServiceLocator.Get<MapGeneratorSystem>().GetWallDoorPrefab, position, _storedRotation, false);
            newWallDoorObject.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetWallHolder);

            // var data = BD.DiscoData.MapData.GetWallDataByCellPos(_storeedCellPosition);
            TH.DiscoData.MapData.RemoveWallData(_storedCellPosition);
            var newData = TH.DiscoData.MapData.AddNewWallData(_storedCellPosition, newWallDoorObject);
            newData.AssignNewID(GameBundle.Instance.FindAItemByID(_storedAssignmentData.AssignedMaterialID) as MaterialItemSo);
            
            TH.TileIndicator.CloseTileIndicator();
        }

        public bool CheckPlaceInput(ToolHelper TH)
        {
            return TH.InputSystem.GetLeftClickOnWorld(InputType.WasPressedThisFrame);
        }

        private void UpdateClosestWall(ToolHelper TH)
        {
            float lastDis = 9999;
            foreach (var wall in TH.DiscoData.MapData.NewWallData.Values)
            {
                var dis = Vector3.Distance(TH.InputSystem.MousePosition, wall.AssignedWall.transform.position);
                
                if (dis < lastDis)
                {
                    _closestAssignmentData = wall;
                    _wallRotation = wall.AssignedWall.transform.rotation;
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