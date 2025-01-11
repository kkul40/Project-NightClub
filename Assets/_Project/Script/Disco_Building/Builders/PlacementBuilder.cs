using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Disco_Building.Builders
{
    public class PlacementBuilder : IBuildingMethod
    {
        public bool PressAndHold { get; }
        public bool isFinished { get; private set; }

        private GameObject _tempObject;
        private List<MeshRenderer> _tempMeshRenderer;
        private PlacementItemSO _storeItemSo;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();
        private Material _storedDiscoFloorMaterial;

        private bool lastValidation = false;
        private bool updateGuard = true;

        public void OnStart(BuildingNeedsData BD)
        {
            _storeItemSo = BD.StoreItemSo as PlacementItemSO;
            _tempObject = Object.Instantiate(_storeItemSo.Prefab, BD.InputSystem.MousePosition, BD.RotationData.rotation);

            if (_tempObject.TryGetComponent(out IPropUnit propUnit))
                Object.Destroy(propUnit);

            _tempMeshRenderer = BD.MaterialColorChanger.ReturnMeshRendererList(_tempObject);

            var transforms = SceneGameObjectHandler.Instance.GetExcludedTransformsByLayer(_storeItemSo.PlacementLayer);
            foreach (var transform in transforms)
                BD.MaterialColorChanger.SetCustomMaterial(transform,
                    MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);

            _storedDiscoFloorMaterial = _tempMeshRenderer[0].material;

            lastValidation = false;
        }

        public bool OnValidate(BuildingNeedsData BD)
        {
            Transform transform = null;
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.FloorProp:
                case ePlacementLayer.BaseSurface:
                    transform = BD.InputSystem.GetHitTransformWithLayer(ConstantVariables.FloorLayerID);
                    if (BD.DiscoData.placementDataHandler.ContainsKeyOnWall(BD.CellPosition, 1)) 
                        return false;
                    break;
                case ePlacementLayer.WallProp:
                    transform = BD.InputSystem.GetHitTransformWithLayer(ConstantVariables.WalllayerID);
                    if (BD.DiscoData.placementDataHandler.ContainsKey(BD.CellPosition, ePlacementLayer.FloorProp))
                        return false;
                    break;
            }

            if (transform == null) return false;

            if (BD.DiscoData.placementDataHandler.ContainsKey(BD.CellPosition,
                    _storeItemSo.Size, BD.RotationData,
                    _storeItemSo.PlacementLayer)) return false;


            return true;
        }

        public void OnUpdate(BuildingNeedsData BD)
        {
            _tempObject.transform.position = Vector3.Lerp(_tempObject.transform.position,
                BD.CellCenterPosition + new Vector3(0.02f, 0.02f, 0.02f),
                Time.deltaTime * BD.MoveSpeed);
            
            _tempObject.transform.rotation = BD.RotationData.rotation;

            bool currentValidation = OnValidate(BD);

            if (currentValidation != lastValidation)
                lastValidation = currentValidation;
            else if(!updateGuard) return;
            
            if (_storeItemSo.PlacementLayer != ePlacementLayer.BaseSurface)
            {
                BD.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, currentValidation);
            }
            else
            {
                if (currentValidation)
                    BD.MaterialColorChanger.SetMaterialColor(_tempMeshRenderer, _storedDiscoFloorMaterial);
                else
                    BD.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, false);
            }

            updateGuard = false;
        }

        public void OnPlace(BuildingNeedsData BD)
        {
            var createdObject = Object.Instantiate(_storeItemSo.Prefab, BD.CellCenterPosition,
                BD.RotationData.rotation);
            
            createdObject.AnimatedPlacement(ePlacementAnimationType.BouncyScaleUp);

            createdObject.transform.SetParent(
                SceneGameObjectHandler.Instance.GetHolderByLayer(_storeItemSo.PlacementLayer));

            BD.DiscoData.placementDataHandler.AddPlacement(BD.CellPosition,
                new PlacementDataHandler.PlacementData(_storeItemSo, BD.CellPosition, createdObject,
                    BD.RotationData));
            
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.FloorProp:
                case ePlacementLayer.BaseSurface:
                    BD.FXCreator.CreateFX(FXType.Floor, BD.CellPosition.GetPlacementCenter(_storeItemSo.Size, BD.RotationData.direction), _storeItemSo.Size, BD.RotationData.rotation);
                    break;
                case ePlacementLayer.WallProp:
                    Vector3 smokeOffset = new Vector3(0, 0.5f, 0);
                    switch (BD.RotationData.direction)
                    {
                        case Direction.Left:
                            smokeOffset.x = -0.5f;
                            break;
                        case Direction.Down:
                            smokeOffset.z = -0.5f;
                            break;
                    }
                    BD.FXCreator.CreateFX(FXType.Floor, BD.CellPosition.GetPlacementCenter(_storeItemSo.Size, BD.RotationData.direction).AddVector(smokeOffset), _storeItemSo.Size, BD.RotationData.rotation.Combine(Quaternion.AngleAxis(90, Vector3.right)));
                    break;
            }
        }

        public void OnStop(BuildingNeedsData BD)
        {
            Object.Destroy(_tempObject);
            BD.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
            isFinished = true;
        }
    }
}