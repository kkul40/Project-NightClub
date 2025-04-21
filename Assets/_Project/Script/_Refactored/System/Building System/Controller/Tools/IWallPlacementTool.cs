using System.Collections.Generic;
using System.Linq;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using ExtensionMethods;
using PropBehaviours;
using Unity.Mathematics;
using UnityEngine;

namespace System.Building_System.Controller.Tools
{
    public class IWallPlacementTool : ITool
    {
        private GameObject _tempObject;
    
        private PlacementItemSO _placementItem;
        private List<MeshRenderer> _tempMeshRenderer;

        private List<WallData> _walls;

        public bool isFinished { get; private set; }

        public void OnStart(ToolHelper TH)
        {
            _placementItem = TH.SelectedStoreItem as PlacementItemSO;

            _tempObject = UnityEngine.Object.Instantiate(_placementItem.Prefab, TH.LastPosition, quaternion.identity);
            _tempObject.transform.SetParent(null);
        
            TH.CalculateBounds(_tempObject.GetComponents<Collider>());

            _walls = TH.DiscoData.MapData.WallDatas.ToList();

            _tempMeshRenderer = TH.MaterialColorChanger.ReturnMeshRendererList(_tempObject);
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (TH.InputSystem.GetHitTransformWithLayer(ToolHelper.WallLayerID) == null) return false;
            if (!TH.HeightCheck()) return false;
            if (!TH.MapBoundryCheck()) return false; 

            var colliders = Physics.OverlapBox(TH.GetCenterOfBounds(), TH.colliderExtend * (ToolHelper.HitCollisionLeniency - 0.02f), TH.LastRotation);
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

                    if (propUnit.PlacementLayer == ePlacementLayer.FloorProp)
                        return false;
                }
            }
        
            return true;
        }

        public void OnUpdate(ToolHelper TH)
        {
            TH.LastPosition = TH.SnapToGrid(TH.InputSystem.MousePosition, _placementItem.GridSizes);
        
            if (TH.InputSystem.FreePlacementKey) // Free Placement
            {
                TH.LastPosition = TH.InputSystem.MousePosition;
            }
        
            WallData closestWall = TH.GetClosestWall();
            if (closestWall != null)
                TH.LastRotation = closestWall.assignedWall.transform.rotation;
        

            _tempObject.transform.rotation = TH.LastRotation;
            _tempObject.transform.position = TH.LastPosition;
        
            TH.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, OnValidate(TH));
        }

        public void OnPlace(ToolHelper TH)
        {
            var obj = UnityEngine.Object.Instantiate(_placementItem.Prefab, TH.LastPosition, TH.LastRotation);
        
            IPropUnit unit;
            if (obj.TryGetComponent(out IPropUnit propUnit))
                unit = propUnit;
            else
                unit = obj.AddComponent<IPropUnit>();

            unit.Initialize(_placementItem.ID, new Vector3Int((int)TH.LastPosition.x, (int)TH.LastPosition.y, (int)TH.LastPosition.z), RotationData.Default, ePlacementLayer.WallProp);
        
            obj.AnimatedPlacement(ePlacementAnimationType.MoveDown);
        
            TH.BuildingController.AddPlacementItemData(_placementItem, obj.transform, TH.LastPosition, TH.LastRotation);

            Quaternion rotation = Quaternion.Euler(90, TH.LastRotation.eulerAngles.y, 0);
            
            TH.FXCreatorSystem.CreateFX(FXType.Floor, TH.LastPosition , new Vector2(TH.colliderSize.x, TH.colliderSize.z), rotation);
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
    }
}