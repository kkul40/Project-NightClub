using System.Collections.Generic;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using PropBehaviours;
using Unity.Mathematics;
using UnityEngine;

namespace System.Building_System.Controller.Tools
{
    public class IFloorPlacementTool : ITool
    {
        private GameObject _tempObject;

        private PlacementItemSO _placementItem;
        private Vector3 mouseDeltaStart;
        private List<MeshRenderer> _tempMeshRenderer;

        private Transform hitSurface;
        private bool isSnappedToSurface;
    
        // Box Collider Values
        // private Collider _collider;

        public bool isFinished { get; private set; }

        public void OnStart(ToolHelper TH)
        {
            _placementItem = TH.SelectedStoreItem as PlacementItemSO;

            _tempObject = UnityEngine.Object.Instantiate(_placementItem.Prefab, TH.LastPosition, quaternion.identity);
            _tempObject.transform.SetParent(null);
        
            TH.CalculateBounds(_tempObject.GetComponents<Collider>());

            _tempMeshRenderer = TH.MaterialColorChanger.ReturnMeshRendererList(_tempObject);
        }
    
        public bool OnValidate(ToolHelper TH)
        {
            // Check For Wall
            // Check For Other Object
            // Check For In Boundries
            // Check For Surface
        
            // Boundry Check
            if (!TH.HeightCheck()) return false;
        
            // Map Boundry
            if (!TH.MapBoundryCheck()) return false;
        
            if (isSnappedToSurface)
                if (!CheckIfPlacedOnSurface(TH)) return false;
        
            // Collision Check
            var colliders = Physics.OverlapBox(TH.GetCenterOfBounds(),TH.colliderExtend * ToolHelper.HitCollisionLeniency, TH.LastRotation);
            for (int i = 0; i < colliders.Length; i++)
            {
                var hitObject = colliders[i];
            
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

            // Surface Placement Check
            if (_placementItem.canPlaceOntoOtherObjects)
            {
                hitSurface = TH.InputSystem.GetHitTransformWithLayer(ToolHelper.SurfaceLayerID);

                if (hitSurface != null)
                    TH.SnapToSurfaceGrid(TH, hitSurface);

                isSnappedToSurface = hitSurface != null;
            }
         
            // Apply To Object
            _tempObject.transform.position = TH.LastPosition;
            _tempObject.transform.rotation = TH.LastRotation;
        
            TH.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, OnValidate(TH));
        }
    
        public void OnPlace(ToolHelper TH)
        {
            if (TH.isReloacting)
            {
                isFinished = true;
                return;
            }
            
            var obj = UnityEngine.Object.Instantiate(_placementItem.Prefab, TH.LastPosition, TH.LastRotation);

            // Setting Parent Object
            if (isSnappedToSurface)
            {
                Transform snapHolder = hitSurface.transform.parent.Find("Snapped Object Holder");

                if (snapHolder == null)
                {
                    GameObject newHolder = new GameObject();
                    newHolder.transform.SetParent(hitSurface.parent);

                    newHolder.name = "Snapped Object Holder";
                    snapHolder = newHolder.transform;
                }
            
                obj.transform.SetParent(snapHolder);
            }
            else
            {
                obj.transform.SetParent(SceneGameObjectHandler.Instance.GetHolderByLayer(_placementItem.PlacementLayer));
            }

            IPropUnit unit;
            if (obj.TryGetComponent(out IPropUnit propUnit))
                unit = propUnit;
            else
                unit = obj.AddComponent<IPropUnit>();

            unit.Initialize(_placementItem.ID, new Vector3Int((int)TH.LastPosition.x, (int)TH.LastPosition.y, (int)TH.LastPosition.z), RotationData.Default, ePlacementLayer.FloorProp);

            TH.BuildingController.AddPlacementItemData(_placementItem, obj.transform, TH.LastPosition, TH.LastRotation);
        }

        public void OnStop(ToolHelper TH)
        {
            if (_tempObject != null)
            {
                UnityEngine.Object.Destroy(_tempObject.gameObject);
            }
        }

        public bool CheckPlaceInput(ToolHelper TH)
        {
            return TH.InputSystem.LeftClickOnWorld;
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        ///

        private void FloorPositioning(ToolHelper TH)
        {
            TH.LastPosition = TH.SnapToGrid(TH.InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID), _placementItem.GridSizes);
            TH.LastPosition.y = 0;

            if (TH.InputSystem.FreePlacementKey) // Free Placement
            {
                TH.LastPosition = InputSystem.Instance.MousePosition;
            }
        }

        private void FloorRotation(ToolHelper TH)
        {
            if (TH.InputSystem.RotateLeft)
                TH.LastRotation = TH.SnappyRotate(_tempObject.transform.rotation, 1);
            else if(TH.InputSystem.RotateRight)
                TH.LastRotation = TH.SnappyRotate(_tempObject.transform.rotation, -1);
        }

        private bool CheckIfPlacedOnSurface(ToolHelper TH)
        {
            foreach (var vector3 in TH.GetRotatedFloorCorners(TH.LastRotation))
            {
                Debug.Log(vector3);
                Ray ray = new Ray(vector3, Vector3.down);
                // TODO + Check If It Hits a surface of a different object.
                if (Physics.CheckSphere(vector3, 0.1f, 1 << ToolHelper.SurfaceLayerID))
                {
                    Debug.DrawRay(ray.origin, ray.direction, Color.green);
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction, Color.red);
                    return false;
                }
            }
            return true;
        }
    }
}