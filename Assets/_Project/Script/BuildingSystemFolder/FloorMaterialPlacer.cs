using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class FloorMaterialPlacer : IBuilder
    {
        [SerializeField] private Transform fx_Wall;
        [SerializeField] private Transform fx_Floor;
        
        private FloorMaterialSo _floorMaterialSo;
        private IMaterial _tempMaterialObject;
        private IMaterial _lastMaterialObject;
        private Transform _lastTransform;
        
        private Material[] savedMaterials;
        
        public void Setup(PlacablePropSo placablePropSo)
        {
            if (placablePropSo is FloorMaterialSo floorMaterialSo)
            {
                _floorMaterialSo = floorMaterialSo;
            }
        }

        public void BuildUpdate()
        {
            BuildingSystem.Instance.GetMouseCellPosition();
            var mousePos = InputSystem.Instance.GetMouseMapPosition();

            if (InputSystem.Instance.GetLastHitTransform().TryGetComponent(out IMaterial tileObject))
            {
                _tempMaterialObject = tileObject;
                _lastTransform = InputSystem.Instance.GetLastHitTransform();
            }
            
            if (_tempMaterialObject != _lastMaterialObject) // Optimization
            {
                // Reset Previous Material
                if (_lastMaterialObject != null)
                {
                    _lastMaterialObject.ResetMaterial(savedMaterials);
                    _lastMaterialObject = null;
                }
            
                // Preview Selected Material On Object
                if (_tempMaterialObject != null)
                {
                    _lastMaterialObject = _tempMaterialObject;
                    savedMaterials = _tempMaterialObject.GetCurrentMaterial();
                    _tempMaterialObject.ChangeMaterial(_floorMaterialSo.Material);
                }
            }
            
            if (InputSystem.Instance.LeftClickOnWorld)
            {
                if (_tempMaterialObject != null)
                {
                    _lastMaterialObject = null;
                }
            }

            if (InputSystem.Instance.Esc)
            {
                Exit();
            }
        }

        public void Exit()
        {
            if (_lastMaterialObject != null)
            {
                _lastMaterialObject.ResetMaterial(savedMaterials);
                _lastMaterialObject = null;
            }
            
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }
    }
}