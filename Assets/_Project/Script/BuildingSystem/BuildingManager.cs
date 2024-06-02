using System;
using BuildingSystem.Builders;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingSystem
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        /*
         *  Item Variants : Props, Material, Map Extender, Background
         *  Placement Variants : Floor, Wall, Surface
         *  Rotation Variants : 360 Degree, Left and Right, No Rotation
         *  Placement Layer : Surface, Floor
         *
         *
         */
        
        private StoreItemSO _storeItemSo;
        private IBuildingMethod _buildingMethod;
        private IRotationMethod _rotationMethod;
        private BuildingNeedsData _buildingNeedsData;
        
        public bool isPlacing => _buildingMethod != null;
        
        [SerializeField] private SceneGameObjectHandler sceneGameObjectHandler;
        [SerializeField] private GridHandler _gridHandler;
        [SerializeField] private MaterialColorChanger _materialColorChanger;

        private void Start()
        {
            _buildingNeedsData = new BuildingNeedsData(InputSystem.Instance, GameData.Instance, sceneGameObjectHandler, _materialColorChanger);
        }

        private void Update()
        {
            if (_buildingMethod != null)
            {
                if (_buildingMethod.isFinished)
                {
                    StopBuild();
                    return;
                }
                
                UpdateBuildingNeeds();
                
                _buildingMethod.OnUpdate(_buildingNeedsData);
                
                if (_buildingMethod.PressAndHold ? InputSystem.Instance.LeftHoldClickOnWorld : InputSystem.Instance.LeftClickOnWorld && _buildingMethod.OnValidate(_buildingNeedsData))
                {
                    _buildingMethod.OnPlace(_buildingNeedsData);
                }
                
                if (InputSystem.Instance.Esc) StopBuild();
            }
        }
        
        private void UpdateBuildingNeeds()
        {
            _buildingNeedsData.CellPosition = _gridHandler.GetMouseCellPosition(InputSystem.Instance.GetMouseMapPosition());
            _buildingNeedsData.CellCenterPosition = _gridHandler.GetCellCenterWorld(_buildingNeedsData.CellPosition) + _buildingMethod.Offset;
            _rotationMethod.Rotate(_buildingNeedsData);
        }
        
        #region Building
        public void StartBuild(StoreItemSO storeItemSo)
        {
            StopBuild();
            
            _storeItemSo = storeItemSo;
            _buildingMethod = BuildingMethodFactory.GetBuildingMethod(storeItemSo);
            _rotationMethod = BuildingMethodFactory.GetRotationMethod(storeItemSo);
            _buildingNeedsData.StoreItemSo = _storeItemSo;
            // _buildingNeedsData.RotationData = new RotationData();
            _buildingMethod.OnStart(_buildingNeedsData);
        }

        public void StopBuild()
        {
            if (_buildingMethod != null)
            {
                _buildingMethod.OnFinish(_buildingNeedsData);
                _buildingMethod = null;
            }
        }
        #endregion
        
        #region Removing
        // TODO Implement Revoving Logic
        #endregion
    }
}