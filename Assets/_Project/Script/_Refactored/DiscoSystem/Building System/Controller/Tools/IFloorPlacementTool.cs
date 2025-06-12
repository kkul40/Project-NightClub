using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using Unity.Mathematics;
using UnityEngine;

namespace DiscoSystem.Building_System.Controller.Tools
{
    public class IFloorPlacementTool : ITool
    {
        private GameObject _tempObject;

        private PlacementItemSO _placementItem;
        private Vector3 mouseDeltaStart;
        private List<MeshRenderer> _tempMeshRenderer;
        private Transform hitSurface;

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        private bool _applyTransformInValidate = true;
        // Box Collider Values
        // private Collider _collider;

        public bool isFinished { get; private set; }

        public void OnStart(ToolHelper TH)
        {
            _placementItem = TH.SelectedStoreItem as PlacementItemSO;

            _tempObject = Object.Instantiate(_placementItem.Prefab, TH.LastPosition, quaternion.identity);
            _tempObject.transform.SetParent(null);
        
            TH.CalculateBounds(_tempObject.GetComponents<Collider>());

            _tempMeshRenderer = TH.MaterialColorChanger.ReturnMeshRendererList(_tempObject);
            TH.TileIndicator.SetTileIndicator(ePlacingType.Place, new Vector2(TH.colliderSize.x, TH.colliderSize.z));
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (_applyTransformInValidate)
            {
                _tempObject.transform.rotation = TH.LastRotation;
                _tempObject.transform.position = TH.LastPosition;
            }
            
            // Check For Wall
            // Check For Other Object
            // Check For In Boundries
            // Check For Surface
        
            // Boundry Check
            if (!TH.HeightCheck()) return false;
        
            // Map Boundry
            if (!TH.MapBoundryCheck()) return false;
        
            // Collision Check
            Collider[] results = new Collider[10];
            var size = Physics.OverlapBoxNonAlloc(TH.GetCenterOfBounds(), TH.colliderExtend * ToolHelper.HitCollisionLeniency, results, TH.LastRotation);
            for (int i = 0; i < size; i++)
            {
                var hitObject = results[i];
            
                if (hitObject.TryGetComponent(out AutoDoor door)) return false;

                var hitUnit = hitObject.GetComponentInParent<IPropUnit>();
                if (hitUnit == null || hitUnit.transform == _tempObject.transform)
                    continue;

                IPropUnit propUnit = hitObject.GetComponentInParent<IPropUnit>();
           
                if (propUnit != null)
                {
                    if (propUnit.PlacementLayer == _placementItem.PlacementLayer)
                        return false;

                    if (propUnit.PlacementLayer == ePlacementLayer.WallProp)
                        return false;
                }
            }
            
            return true;
        }
    
        public void OnUpdate(ToolHelper TH)
        {
            FloorRotation(TH);
            FloorPositioning(TH);
            
            // Apply To Object
            _tempObject.transform.position = TH.MoveObjectToLastPosition(_tempObject.transform.position);
            _tempObject.transform.rotation = TH.RotateObjectToLastRotation(_tempObject.transform.rotation);
            
            // _tempObject.transform.position = TH.LastPosition;
            // _tempObject.transform.rotation = TH.LastRotation;
            
            DebugExtension.DrawBox(TH.GetCenterOfBounds(),TH.colliderSize, TH.LastRotation, Color.cyan, 0.2f);

            _applyTransformInValidate = false;
            TH.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, OnValidate(TH));
            _applyTransformInValidate = true;
            TH.TileIndicator.SetPositionAndRotation(TH.LastPosition, TH.LastRotation);
        }
    
        public void OnPlace(ToolHelper TH)
        {
            // _tempObject.transform.position = TH.LastPosition;
            // _tempObject.transform.rotation = TH.LastRotation;
            
            var obj = Object.Instantiate(_placementItem.Prefab, TH.LastPosition, TH.LastRotation);

            IPropUnit unit;
            if (obj.TryGetComponent(out IPropUnit propUnit))
                unit = propUnit;
            else
                unit = obj.AddComponent<IPropUnit>();

            unit.Initialize(_placementItem.ID, ePlacementLayer.FloorProp);
            
            Physics.SyncTransforms();
            TH.BuildingController.AddPlacementItemData(_placementItem, obj.transform, TH.LastPosition, TH.LastRotation, TH.colliderSize, TH.GetPlacedPosition());
            TH.FXCreatorSystem.CreateFX(FXType.Floor, TH.LastPosition, new Vector2(TH.colliderSize.x, TH.colliderSize.z), TH.LastRotation);
            TH.PlacementTracker.AddTrack(new PropUndo(_placementItem.ID, obj.transform.GetInstanceID()));
        }

        public void OnStop(ToolHelper TH)
        {
            if (_tempObject != null)
                Object.Destroy(_tempObject.gameObject);
            
            TH.TileIndicator.CloseTileIndicator();
        }

        public bool CheckPlaceInput(ToolHelper TH)
        {
            return TH.InputSystem.GetLeftClickOnWorld(InputType.WasPressedThisFrame);
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        ///

        private void FloorPositioning(ToolHelper TH)
        {
            TH.LastPosition = TH.SnapToGrid(TH.InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID), _placementItem.GridSizes);
            TH.LastPosition.y = 0;

            if (TH.InputSystem.GetFreePlacement(InputType.InProggress)) // Free Placement
            {
                TH.LastPosition = InputSystem.Instance.MousePosition;
            }
        }

        private void FloorRotation(ToolHelper TH)
        {
            if (TH.InputSystem.GetRotationInput(InputType.WasPressedThisFrame))
            {
                TH.LastRotation = TH.SnappyRotate(_tempObject.transform.rotation, TH.InputSystem.GetRotation());
            }
        }
    }
}