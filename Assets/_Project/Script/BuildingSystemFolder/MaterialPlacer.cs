using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class MaterialPlacer : MonoBehaviour, IBuild
    {
        [SerializeField] private Transform fx_Wall;
        [SerializeField] private Transform fx_Floor;
        
        private MaterialSo _tempMaterialSo;
        private IMaterial _tempMaterialObject;
        private IMaterial _lastMaterialObject;
        private Transform _lastTransform;
        
        private Material[] savedMaterials;
        
        public void Setup<T>(T itemSo) where T : ItemSo
        {
            if (itemSo is MaterialSo wallPaperSo)
            {
                _tempMaterialSo = wallPaperSo;
            }
        }

        public void BuildUpdate()
        {
            BuildingSystem.Instance.GetMouseCellPosition();
            var mousePos = InputSystem.Instance.GetMouseMapPosition();

            // Get TEMP Material Object
            switch (_tempMaterialSo.MaterialType)
            {
                case MaterialType.WallPaper:
                    _tempMaterialObject = GetClosestWallMaterial(mousePos);
                    break;
                case MaterialType.Floor:
                    if (InputSystem.Instance.GetLastHitTransform().TryGetComponent(out IMaterial tileObject))
                    {
                        _tempMaterialObject = tileObject;
                        _lastTransform = InputSystem.Instance.GetLastHitTransform();
                    }
                    else // Reset Material IF Cursor Out of Boundries
                    {
                        _tempMaterialObject = null;
                        if (_lastMaterialObject != null)
                        {
                            _lastMaterialObject.ResetMaterial(savedMaterials);
                        }
                    }
                    break;
                default:
                    _tempMaterialObject = null;
                    break;
            }
           
            /*
             * materyaller aynimi diye kontrol et
             * 
             */
            
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
                    _tempMaterialObject.ChangeMaterial(_tempMaterialSo.Material);
                }
            }
            
            if (InputSystem.Instance.LeftClickOnWorld)
            {
                if (savedMaterials.SequenceEqual(_tempMaterialObject.GetCurrentMaterial()))
                {
                    Debug.Log("TEst");
                }
                if (_tempMaterialObject != null)
                {
                    switch (_tempMaterialSo.MaterialType)
                    {
                        case MaterialType.WallPaper:
                            BuildingSystem.Instance.PlayFX(fx_Wall, _lastTransform.position, _lastTransform.rotation);
                            break;
                        case MaterialType.Floor:
                            BuildingSystem.Instance.PlayFX(fx_Floor, _lastTransform.position, _lastTransform.rotation);
                            break;
                    }
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
        
        private IMaterial GetClosestWallMaterial(Vector3 cellPos)
        {
            float lastDis = 9999;
            IMaterial closestWallMaterial = null;
            foreach(var wall in GameData.Instance.GetWallMapPosList())
            {
                var dis = Vector3.Distance(cellPos, wall.transform.position);
                if (dis < lastDis)
                {
                    closestWallMaterial = wall as IMaterial;
                    lastDis = dis;
                    _lastTransform = wall.transform;
                }
            }

            return closestWallMaterial;
        }
    }
}