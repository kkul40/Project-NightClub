using System.Collections.Generic;
using BuildingSystem.SO;
using UnityEngine;

namespace BuildingSystem.Builders
{
    public class MaterialBuilder : IBuildingMethod
    {
        public bool PressAndHold { get; } = true;
        public bool isFinished { get; }
        public Vector3 Offset { get; }

        private MaterialItemSo _materialItemSo;
        private IChangableMaterial _changableMaterial;
        private IChangableMaterial _lastChangableMaterial;
        private Material _previousMaterial;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas =
            new Dictionary<Transform, MaterialColorChanger.MaterialData>();
        
        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            _materialItemSo = buildingNeedsData.StoreItemSo as MaterialItemSo;
            
            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.Wall:
                    buildingNeedsData.MaterialColorChanger.SetCustomMaterial(buildingNeedsData.SceneGameObjectHandler.PropHolderTransform, MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
                    return;
                case eMaterialLayer.FloorTile:
                    buildingNeedsData.MaterialColorChanger.SetCustomMaterial(buildingNeedsData.SceneGameObjectHandler.SurfaceHolderTransform, MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
                    buildingNeedsData.MaterialColorChanger.SetCustomMaterial(buildingNeedsData.SceneGameObjectHandler.PropHolderTransform, MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
                    return;
            }
        }

        public bool OnValidate(BuildingNeedsData buildingNeedsData)
        {
            return true;
        }

        public void OnUpdate(BuildingNeedsData buildingNeedsData)
        {
            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.FloorTile:
                    _changableMaterial = FindMaterial(buildingNeedsData);
                    break;
                case eMaterialLayer.Wall:
                    _changableMaterial = GetClosestWallMaterial(buildingNeedsData);
                    break;
            }

            if (_changableMaterial == null)
            {
                ResetPreviousMaterial();
                return;
            }

            if (_changableMaterial != _lastChangableMaterial)
            {
                ResetPreviousMaterial();
                _lastChangableMaterial = _changableMaterial;
                
                _previousMaterial = _changableMaterial.CurrentMaterial;
                _changableMaterial.CurrentMaterial = _materialItemSo.Material;
                _changableMaterial.UpdateMaterial();
            }
        }

        public void OnPlace(BuildingNeedsData buildingNeedsData)
        {
            _lastChangableMaterial = null;
        }

        public void OnFinish(BuildingNeedsData buildingNeedsData)
        {
           ResetPreviousMaterial();
           buildingNeedsData.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
        }

        private IChangableMaterial FindMaterial(BuildingNeedsData buildingNeedsData)
        {
            Transform hitTransform = null;
            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.FloorTile:
                    hitTransform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(7);
                    break;
                case eMaterialLayer.Wall:
                    hitTransform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(8);
                    break;
            }
            
            if (hitTransform != null && hitTransform.TryGetComponent(out IChangableMaterial changableMaterial))
            {
                return changableMaterial;
            }

            return null;
        }

        private void ResetPreviousMaterial()
        {
            if (_lastChangableMaterial == null) return;
            _lastChangableMaterial.CurrentMaterial = _previousMaterial;
            _lastChangableMaterial.UpdateMaterial();
            _lastChangableMaterial = null;
        }
        
        private IChangableMaterial GetClosestWallMaterial(BuildingNeedsData buildingNeedsData)
         {
             float lastDis = 9999;
             IChangableMaterial closestChangableMaterial = null;
             foreach(var wall in buildingNeedsData.DiscoData.mapData.GetWallMapPosList())
             {
                 var dis = Vector3.Distance(buildingNeedsData.InputSystem.GetMouseMapPosition(), wall.transform.position);
                 if (dis < lastDis)
                 {
                     closestChangableMaterial = wall as IChangableMaterial;
                     lastDis = dis;
                 }
             }

             return closestChangableMaterial;
         }
    }
}