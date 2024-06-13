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
        private Func<Action> callBackOnPlace = null;
        
        [SerializeField] private SceneGameObjectHandler sceneGameObjectHandler;
        [SerializeField] private GridHandler _gridHandler;
        [SerializeField] private MaterialColorChanger _materialColorChanger;
        
        private void Start()
        {
            _buildingNeedsData = new BuildingNeedsData(InputSystem.Instance, DiscoData.Instance, sceneGameObjectHandler, _materialColorChanger);
            _gridHandler.ToggleGrid(false);
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
                
                _rotationMethod.OnRotate(_buildingNeedsData);
                _buildingMethod.OnUpdate(_buildingNeedsData);
                
                if (_buildingMethod.PressAndHold ? InputSystem.Instance.LeftHoldClickOnWorld : InputSystem.Instance.LeftClickOnWorld && _buildingMethod.OnValidate(_buildingNeedsData))
                {
                    _buildingMethod.OnPlace(_buildingNeedsData);
                    callBackOnPlace?.Invoke().Invoke();
                }
                
                if (InputSystem.Instance.Esc) StopBuild();
            }
        }
        
        private void UpdateBuildingNeeds()
        {
            _buildingNeedsData.CellPosition = _gridHandler.GetMouseCellPosition(InputSystem.Instance.GetMouseMapPosition());
            _buildingNeedsData.CellCenterPosition = _gridHandler.GetCellCenterWorld(_buildingNeedsData.CellPosition) + _buildingMethod.Offset;
        }
        
        #region Building

        public void StartRemoving()
        {
            StopBuild();
            _gridHandler.ToggleGrid(true);
            _storeItemSo = null;
            _rotationMethod = new NullRotationMethod();
            _buildingMethod = new RemoveHandler();
            _buildingNeedsData.StoreItemSo = _storeItemSo;
            _rotationMethod.OnStart(_buildingNeedsData);
            _buildingMethod.OnStart(_buildingNeedsData);
        }
        public void StartBuild(StoreItemSO storeItemSo, Func<Action> CallBackOnPlace = null)
        {
            StopBuild();
            callBackOnPlace = CallBackOnPlace;
            _gridHandler.ToggleGrid(true);
            _storeItemSo = storeItemSo;
            _rotationMethod = BuildingMethodFactory.GetRotationMethod(storeItemSo);
            _buildingMethod = BuildingMethodFactory.GetBuildingMethod(storeItemSo);
            _buildingNeedsData.StoreItemSo = _storeItemSo;
            _rotationMethod.OnStart(_buildingNeedsData);
            _buildingMethod.OnStart(_buildingNeedsData);
        }

        public void StopBuild()
        {
            if (_buildingMethod != null)
            {
                _buildingMethod.OnFinish(_buildingNeedsData);
                _buildingMethod = null;
                _rotationMethod = null;
            }
            _gridHandler.ToggleGrid(false);
            callBackOnPlace = null;
        }
        #endregion
    }
}