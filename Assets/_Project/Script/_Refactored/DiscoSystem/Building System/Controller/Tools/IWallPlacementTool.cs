using System.Collections.Generic;
using System.Linq;
using Data;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using Unity.Mathematics;
using UnityEngine;

namespace DiscoSystem.Building_System.Controller.Tools
{
    public class IWallPlacementTool : ITool
    {
        private GameObject _tempObject;
    
        private PlacementItemSO _placementItem;
        private List<MeshRenderer> _tempMeshRenderer;

        private List<WallData> _walls;
        
        private bool _applyTransformInValidate = true;

        public bool isFinished { get; private set; }

        public void OnStart(ToolHelper TH)
        {
            _placementItem = TH.SelectedStoreItem as PlacementItemSO;

            _tempObject = Object.Instantiate(_placementItem.Prefab, TH.LastPosition, quaternion.identity);
            _tempObject.transform.SetParent(null);
        
            TH.CalculateBounds(_tempObject.GetComponents<Collider>());

            _walls = TH.DiscoData.MapData.NewWallData.Values.ToList();

            _tempMeshRenderer = TH.MaterialColorChanger.ReturnMeshRendererList(_tempObject);
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (_applyTransformInValidate)
            {
                _tempObject.transform.rotation = TH.LastRotation;
                _tempObject.transform.position = TH.LastPosition;
            }
            
            if (TH.InputSystem.GetHitTransformWithLayer(ToolHelper.WallLayerID) == null) return false;
            if (!TH.HeightCheck()) return false;
            if (!TH.MapBoundryCheck()) return false;

            Collider[] results = new Collider[10];
            var size = Physics.OverlapBoxNonAlloc(TH.GetCenterOfBounds(), TH.colliderExtend * (ToolHelper.HitCollisionLeniency - 0.02f), results, TH.LastRotation);
            for (int i = 0; i < size; i++)
            {
                var hitObject = results[i];

                if (hitObject.TryGetComponent(out AutoDoor door)) return false;
            
                var hitUnit = hitObject.GetComponentInParent<IPropUnit>();
                if (hitUnit == null || hitUnit.transform.GetInstanceID() == _tempObject.transform.GetInstanceID())
                {
                    Debug.Log("Kendine Carpiyor");
                    continue;
                }

                IPropUnit propUnit = hitObject.GetComponentInParent<IPropUnit>();
           
                if (propUnit != null)
                {
                    if (propUnit.PlacementLayer == _placementItem.PlacementLayer)
                        return false;

                    if (propUnit.PlacementLayer == ePlacementLayer.FloorProp)
                        return false;
                }
            }
        
            return true;
        }

        public void OnUpdate(ToolHelper TH)
        {
            TH.LastPosition = TH.SnapToGrid(TH.InputSystem.MousePosition, _placementItem.GridSizes);
        
            if (TH.InputSystem.GetFreePlacement(InputType.InProggress)) // Free Placement
            {
                TH.LastPosition = TH.InputSystem.MousePosition;
            }
        
            WallData closestWall = TH.GetClosestWall();
            if (closestWall != null)
                TH.LastRotation = closestWall.AssignedWall.transform.rotation;
            
            _tempObject.transform.position = TH.MoveObjectToLastPosition(_tempObject.transform.position);
            _tempObject.transform.rotation = TH.RotateObjectToLastRotation(_tempObject.transform.rotation);

            _applyTransformInValidate = false;
            TH.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, OnValidate(TH));
            _applyTransformInValidate = true;
        }

        public void OnPlace(ToolHelper TH)
        {
            var obj = Object.Instantiate(_placementItem.Prefab, TH.LastPosition, TH.LastRotation);
        
            IPropUnit unit;
            if (obj.TryGetComponent(out IPropUnit propUnit))
                unit = propUnit;
            else
                unit = obj.AddComponent<IPropUnit>();

            unit.Initialize(_placementItem.ID, ePlacementLayer.WallProp);
        
            obj.AnimatedPlacement(ePlacementAnimationType.MoveDown);
        
            TH.BuildingController.AddPlacementItemData(_placementItem, obj.transform, TH.LastPosition, TH.LastRotation);

            Quaternion rotation = Quaternion.Euler(90, TH.LastRotation.eulerAngles.y, 0);
            
            TH.FXCreatorSystem.CreateFX(FXType.Floor, TH.LastPosition , new Vector2(TH.colliderSize.x, TH.colliderSize.z), rotation);
            TH.PlacementTracker.AddTrack(new PropUndo(_placementItem.ID, obj.transform.GetInstanceID()));
        }

        public void OnStop(ToolHelper TH)
        {
            if (_tempObject != null)
            {
                Object.Destroy(_tempObject.gameObject);
            }
        }

        public bool CheckPlaceInput(ToolHelper TH)
        {
            return TH.InputSystem.GetLeftClickOnWorld(InputType.WasPressedThisFrame);
        }
    }
}