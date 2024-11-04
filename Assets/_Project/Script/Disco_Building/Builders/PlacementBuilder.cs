using System;
using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using PropBehaviours;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BuildingSystem.Builders
{
    public class PlacementBuilder : IBuildingMethod
    {
        public bool PressAndHold { get; }
        public bool isFinished { get; private set; }

        private GameObject _tempObject;
        private List<MeshRenderer> _tempMeshRenderer;
        private PlacementItemSO _storeItemSo;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public void OnStart(BuildingNeedsData BD)
        {
            _storeItemSo = BD.StoreItemSo as PlacementItemSO;
            _tempObject =
                Object.Instantiate(_storeItemSo.Prefab, Vector3.zero, BD.RotationData.rotation);

            if (_tempObject.TryGetComponent(out IPropUnit propUnit))
                Object.Destroy(propUnit);

            _tempMeshRenderer = BD.MaterialColorChanger.ReturnMeshRendererList(_tempObject);

            var transforms = SceneGameObjectHandler.Instance.GetExcludeTransformsByLayer(_storeItemSo.PlacementLayer);
            foreach (var transform in transforms)
                BD.MaterialColorChanger.SetCustomMaterial(transform,
                    MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
        }

        public bool OnValidate(BuildingNeedsData BD)
        {
            Transform transform = null;
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.FloorProp:
                case ePlacementLayer.BaseSurface:
                    transform = BD.InputSystem.GetHitTransformWithLayer(ConstantVariables.FloorLayerID);
                    break;
                case ePlacementLayer.WallProp:
                    transform = BD.InputSystem.GetHitTransformWithLayer(ConstantVariables.WalllayerID);
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

            BD.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer,
                OnValidate(BD));
        }

        public void OnPlace(BuildingNeedsData BD)
        {
            var createdObject = Object.Instantiate(_storeItemSo.Prefab, BD.CellCenterPosition,
                BD.RotationData.rotation);
            
            createdObject.AnimatedPlacement(BD.CellCenterPosition);

            createdObject.transform.SetParent(
                SceneGameObjectHandler.Instance.GetHolderByLayer(_storeItemSo.PlacementLayer));

            BD.DiscoData.placementDataHandler.AddPlacement(BD.CellPosition,
                new PlacementDataHandler.PlacementData(_storeItemSo, BD.CellPosition, createdObject,
                    BD.RotationData));
            
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.FloorProp:
                case ePlacementLayer.BaseSurface:
                    BD.FXCreator.CreateFX(FXType.Floor, BD.CellPosition.CellCenterPosition(eGridType.PlacementGrid), _storeItemSo.Size, BD.RotationData.rotation);
                    break;
                case ePlacementLayer.WallProp:
                    // TODO Duvara yerlestirirken onune effect yapmak yerine duvar tarafindan olustur effekti
                    BD.FXCreator.CreateFX(FXType.Floor, BD.CellPosition.CellCenterPosition(eGridType.PlacementGrid).Add(y:0.5f), _storeItemSo.Size, BD.RotationData.rotation.Combine(Quaternion.AngleAxis(90, Vector3.right)));
                    break;
            }
        }

        public void OnStop(BuildingNeedsData BD)
        {
            Object.Destroy(_tempObject);
            isFinished = true;
            BD.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
        }
    }
}