using System;
using Data;
using Disco_Building.Builders;
using Disco_ScriptableObject;
using ExtensionMethods;
using UI.GamePages;
using UnityEngine;

namespace Disco_Building
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
        [SerializeField] private FXCreator _fxCreator;


        public static event Action OnRemovingStopped;

        private void Start()
        {
            _buildingNeedsData = new BuildingNeedsData(InputSystem.Instance, DiscoData.Instance, _materialColorChanger, _fxCreator);
            HandleToggling(false);
        }

        private void OnEnable()
        {
            UIStorePage.OnStoreToggle += HandleToggling;
        }

        private void OnDisable()
        {
            UIStorePage.OnStoreToggle -= HandleToggling;
        }

        private void HandleToggling(bool toggle)
        {
            _gridHandler.ToggleGrid(toggle);
            if(!toggle) StopBuild();
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

                if ((_buildingMethod.PressAndHold ? InputSystem.Instance.LeftHoldClickOnWorld : InputSystem.Instance.LeftClickOnWorld)
                    && _buildingMethod.OnValidate(_buildingNeedsData))
                {
                    _buildingMethod.OnPlace(_buildingNeedsData);
                    callBackOnPlaced?.Invoke().Invoke();
                    if (_buildingNeedsData.isReplacing)
                    {
                        StopBuild();
                    }
                }

                if (InputSystem.Instance.RightClickOnWorld) StopBuild();
            }
        }

        private void UpdateBuildingNeeds()
        {
            _buildingNeedsData.CellPosition = InputSystem.Instance.MousePosition.WorldPosToCellPos(eGridType.PlacementGrid);
            _buildingNeedsData.CellCenterPosition = _buildingNeedsData.CellPosition.CellCenterPosition(eGridType.PlacementGrid);
            
            _tileIndicator.SetPosition(_buildingNeedsData.CellPosition);
            _tileIndicator.RoateDirectionIndicator(_buildingNeedsData.RotationData.rotation);
        }

        #region Building

        public void StartRemoving()
        {
            StopBuild();
            _storeItemSo = null;
            _rotationMethod = new NullRotationMethod();
            _buildingMethod = new RemoveHandler();
            _buildingNeedsData.StoreItemSo = _storeItemSo;
            _rotationMethod.OnStart(_buildingNeedsData);
            _buildingMethod.OnStart(_buildingNeedsData);
            _tileIndicator.SetTileIndicator(ePlacingType.Remove);
        }

        public void StartBuild(StoreItemSO storeItemSo, bool? isReplacing = false, Func<Action> CallBackOnPlace = null)
        {
            StopBuild();
            callBackOnPlaced = CallBackOnPlace;
            _storeItemSo = storeItemSo;
            _rotationMethod = Builder.BuildToIRotation(storeItemSo);
            _buildingMethod = Builder.BuildToIBuilding(storeItemSo);
            _buildingNeedsData.StoreItemSo = _storeItemSo;
            _buildingNeedsData.isReplacing = isReplacing ?? false;
            _rotationMethod.OnStart(_buildingNeedsData);
            _buildingMethod.OnStart(_buildingNeedsData);

            _tileIndicator.SetTileIndicator(ePlacingType.Place);
            if (storeItemSo is PlacementItemSO placementItemSo) _tileIndicator.SetSize(placementItemSo.Size);
        }

        public void StopBuild()
        {
            if (_buildingMethod != null)
            {
                if (_buildingMethod is RemoveHandler)
                    OnRemovingStopped?.Invoke();
                
                _buildingMethod.OnStop(_buildingNeedsData);
                _buildingMethod = null;
                _rotationMethod = null;
            }

            callBackOnPlaced = null;
            _tileIndicator.SetSize(Vector2.one);
            _tileIndicator.CloseTileIndicator();
        }

        public void ReplaceObject(StoreItemSO storeItemSo ,Vector3Int cellPos, ePlacementLayer moveFromLayer)
        {
            StopBuild();
            DiscoData.Instance.placementDataHandler.RemovePlacement(cellPos, moveFromLayer, false, true);
            StartBuild(storeItemSo, true);
        }
        #endregion

    }
}