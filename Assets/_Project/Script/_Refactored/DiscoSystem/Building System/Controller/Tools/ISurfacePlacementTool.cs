using System.Collections.Generic;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using Unity.Mathematics;
using UnityEngine;

namespace DiscoSystem.Building_System.Controller.Tools
{
    public class ISurfacePlacementTool : ITool
    {
        private GameObject _tempObject;
        private PlacementItemSO _placementItem;
        private List<MeshRenderer> _tempMeshRenderer;

        public bool isFinished { get; private set; }

        public void OnStart(ToolHelper TH)
        {
            _placementItem = TH.SelectedStoreItem as PlacementItemSO;
            
            TH.LastRotation = quaternion.identity;
        
            _tempObject = UnityEngine.Object.Instantiate(_placementItem.Prefab, TH.LastPosition, quaternion.identity);
            _tempObject.transform.SetParent(null);
        
            TH.CalculateBounds(_tempObject.GetComponents<Collider>());
        
            _tempMeshRenderer = TH.MaterialColorChanger.ReturnMeshRendererList(_tempObject);
            
            TH.TileIndicator.SetTileIndicator(ePlacingType.Place, Vector2.one);
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (TH.InputSystem.GetHitTransformWithLayer(ToolHelper.FloorLayerID) == null)
                return false;
            
            var gridData = TH.DiscoData.MapData.GetFloorGridData((int)TH.LastPosition.x, (int)TH.LastPosition.z);
            if (gridData == null)
                return false;
        
            var colliders = Physics.OverlapBox(TH.GetCenterOfBounds(),TH.colliderExtend * 0.98f, TH.LastRotation);
            for (int i = 0; i < colliders.Length; i++)
            {
                var hitObject = colliders[i];

                var hitUnit = hitObject.GetComponentInParent<IPropUnit>();
                if (hitUnit == null || hitUnit.transform == _tempObject.transform)
                    continue;

                if (hitObject.TryGetComponent(out Wall wall))
                    return false;

                IPropUnit propUnit = hitObject.GetComponentInParent<IPropUnit>();
           
                if (propUnit != null)
                {
                    if (propUnit.PlacementLayer == _placementItem.PlacementLayer)
                        return false;
                }
            }
            return true;
        }

        public void OnUpdate(ToolHelper TH)
        {
            Vector3 mousePos = TH.InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID);
        
            bool validation = OnValidate(TH);

            var gridData = TH.DiscoData.MapData.GetFloorGridData((int)mousePos.x, (int)mousePos.z);
        
            if (gridData != null)
            {
                TH.LastPosition = gridData.CellPosition.CellCenterPosition(eGridType.PlacementGrid);
            }
        
            TH.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, validation);

            _tempObject.transform.position = TH.LastPosition;
            
            TH.TileIndicator.SetPositionAndRotation(TH.LastPosition, Quaternion.identity);
        }

        public void OnPlace(ToolHelper TH)
        {
            var obj = UnityEngine.Object.Instantiate(_placementItem.Prefab, TH.LastPosition, TH.LastRotation);
        
            IPropUnit unit;
            if (obj.TryGetComponent(out IPropUnit propUnit))
                unit = propUnit;
            else
                unit = obj.AddComponent<IPropUnit>();

            unit.Initialize(_placementItem.ID, new Vector3Int((int)TH.LastPosition.x, (int)TH.LastPosition.y, (int)TH.LastPosition.z), ePlacementLayer.BaseSurface);
        
            TH.BuildingController.AddPlacementItemData(_placementItem, obj.transform, TH.LastPosition, TH.LastRotation);
            TH.FXCreatorSystem.CreateFX(FXType.Floor, TH.LastPosition, new Vector2(TH.colliderSize.x, TH.colliderSize.z), TH.LastRotation);
            TH.PlacementTracker.AddTrack(new PropUndo(_placementItem.ID, obj.transform.GetInstanceID()));
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