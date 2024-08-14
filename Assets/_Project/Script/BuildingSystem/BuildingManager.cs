using System;
using BuildingSystem.Builders;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SubsystemsImplementation;

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

        /*
         * DataTypes - ReferanceType
         *
         * Event
         *
         * OOP {Abstraction, Polymorphizm, } Object Orriented Proggramming
         *
         * Project Structure
         *
         * Design Pattern {Singleton, Observer, Dirty Flag, Flyweight, Object Polling, Command Pattern ,Strategic Pattern, Prototype Pattern}
         *
         */

        private StoreItemSO _storeItemSo;
        private IBuildingMethod _buildingMethod;
        private IRotationMethod _rotationMethod;
        private BuildingNeedsData _buildingNeedsData;

        public bool isPlacing => _buildingMethod != null;
        private Func<Action> callBackOnPlaced = null;

        [SerializeField] private TileIndicator _tileIndicator;
        [SerializeField] private SceneGameObjectHandler sceneGameObjectHandler;
        [SerializeField] private GridHandler _gridHandler;
        [SerializeField] private MaterialColorChanger _materialColorChanger;

        private void Start()
        {
            _buildingNeedsData = new BuildingNeedsData(InputSystem.Instance, DiscoData.Instance, _materialColorChanger);
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

                if ((_buildingMethod.PressAndHold
                        ? InputSystem.Instance.LeftHoldClickOnWorld
                        : InputSystem.Instance.LeftClickOnWorld)
                    && _buildingMethod.OnValidate(_buildingNeedsData))
                {
                    _buildingMethod.OnPlace(_buildingNeedsData);
                    callBackOnPlaced?.Invoke().Invoke();
                }

                if (InputSystem.Instance.Esc) StopBuild();
            }
        }

        private void UpdateBuildingNeeds()
        {
            _buildingNeedsData.CellPosition =
                _gridHandler.GetMouseCellPosition(InputSystem.Instance.GetMouseMapPosition(), eGridType.PlacementGrid);

            _buildingNeedsData.CellCenterPosition =
                _gridHandler.GetCellCenterWorld(_buildingNeedsData.CellPosition, eGridType.PlacementGrid) +
                _buildingMethod.Offset;
            _tileIndicator.SetPosition(_buildingNeedsData.CellPosition);
            _tileIndicator.RoateDirectionIndicator(_buildingNeedsData.RotationData.rotation);
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
            _tileIndicator.SetTileIndicator(ePlacingType.Remove);
        }

        public void StartBuild(StoreItemSO storeItemSo, Func<Action> CallBackOnPlace = null)
        {
            StopBuild();
            callBackOnPlaced = CallBackOnPlace;
            _gridHandler.ToggleGrid(true);
            _storeItemSo = storeItemSo;
            _rotationMethod = Builder.BuildToIRotation(storeItemSo);
            _buildingMethod = Builder.BuildToIBuilding(storeItemSo);
            _buildingNeedsData.StoreItemSo = _storeItemSo;
            _rotationMethod.OnStart(_buildingNeedsData);
            _buildingMethod.OnStart(_buildingNeedsData);

            _tileIndicator.SetTileIndicator(ePlacingType.Place);
            if (storeItemSo is PlacementItemSO placementItemSo) _tileIndicator.SetSize(placementItemSo.Size);
        }

        public void StopBuild()
        {
            if (_buildingMethod != null)
            {
                _buildingMethod.OnStop(_buildingNeedsData);
                _buildingMethod = null;
                _rotationMethod = null;
            }

            _gridHandler.ToggleGrid(false);
            callBackOnPlaced = null;
            _tileIndicator.SetSize(Vector2.one);
            _tileIndicator.CloseTileIndicator();
        }

        #endregion

        public override void Initialize()
        {
            Initialize();
        }
    }
}