using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class TileMaterialPlacer : MonoBehaviour, IBuild
    {
        [SerializeField] private Transform fx_Wall;
        [SerializeField] private Transform fx_Floor;
        
        private MaterialSo _tempMaterialSo;
        private TileObject _tempTileObject;
        private TileObject _lastTileObject;

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

            switch (_tempMaterialSo.MaterialType)
            {
                case MaterialType.WallPaper:
                    _tempTileObject = GetClosestWall(mousePos);
                    break;
                case MaterialType.Floor:
                    if (InputSystem.Instance.GetLastHitTransform().TryGetComponent(out TileObject tileObject))
                    {
                        _tempTileObject = tileObject;
                    }
                    else
                    {
                        _tempTileObject = null;
                        if (_lastTileObject != null)
                        {
                            _lastTileObject.ResetMaterial(savedMaterials);
                        }
                    }
                    
                    break;
                default:
                    _tempTileObject = null;
                    break;
            }
           
            if (_tempTileObject != _lastTileObject) // Optimization
            {
                if (_lastTileObject != null)
                {
                    _lastTileObject.ResetMaterial(savedMaterials);
                    _lastTileObject = null;
                }
            
                if (_tempTileObject != null)
                {
                    _lastTileObject = _tempTileObject;
                    savedMaterials = _tempTileObject.GetCurrentMaterial();
                    _tempTileObject.ChangeMaterial(_tempMaterialSo.Material);
                }
            }
            
            if (InputSystem.Instance.LeftClickOnWorld)
            {
                if (_tempTileObject != null)
                {
                    switch (_tempMaterialSo.MaterialType)
                    {
                        case MaterialType.WallPaper:
                            BuildingSystem.Instance.PlayFX(fx_Wall, _tempTileObject.transform.position, _tempTileObject.transform.rotation);
                            break;
                        case MaterialType.Floor:
                            BuildingSystem.Instance.PlayFX(fx_Floor, _tempTileObject.transform.position, _tempTileObject.transform.rotation);
                            break;
                    }
                    // tempWall.ChangeWallpaper(tempWallPaperSo.Material);
                    _lastTileObject = null;
                }
            }

            if (InputSystem.Instance.Esc)
            {
                Exit();
            }
        }

        public void Exit()
        {
            if (_lastTileObject != null)
            {
                _lastTileObject.ResetMaterial(savedMaterials);
                _lastTileObject = null;
            }
            
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }
        
        private Wall GetClosestWall(Vector3 cellPos)
        {
            float lastDis = 9999;
            Wall closestTile = null;
            foreach(var wall in GameData.Instance.GetWallMapPosList())
            {
                var dis = Vector3.Distance(cellPos, wall.transform.position);
                if (dis < lastDis)
                {
                    closestTile = wall as Wall;
                    lastDis = dis;
                }
            }

            return closestTile;
        }
    }
}