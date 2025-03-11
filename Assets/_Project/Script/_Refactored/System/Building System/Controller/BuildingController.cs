using System.Building_System.Controller.Tools;
using System.Building_System.Model;
using System.Building_System.Service;
using System.Building_System.View;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using Framework.Context;
using Framework.Mvcs.Controller;
using GameEvents;
using PropBehaviours;
using UnityEngine;

namespace System.Building_System.Controller
{
    public enum GridSizes
    {
        FullGrid, // 1 x 1
        HalfGrid, // 0.5 x 0.5f
    }

    public class BuildingController : BaseController<BuildingModel, BuildingView, BuildingService>
    {
        private ToolHelper _toolHelper;
        private ITool currentTool;

        public BuildingController(BuildingModel model, BuildingView view, BuildingService service, InputSystem inputSystem,
            DiscoData discoData, MaterialColorChanger materialColorChanger, FXCreatorSystem fxCreatorSystem) : base(model, view,
            service)
        {
            _toolHelper = new ToolHelper(this, inputSystem, discoData, materialColorChanger, fxCreatorSystem);
        }

        public override void Initialize(IContext context)
        {
            base.Initialize(context);

            _view.GenerateItems(_model.StoreItems);
            _view.OnSlotItemClicked += StartATool;
            _view.OnStorageItemClicked += StartInventoryItemPlacement;
            
            KEvent_Building.OnPlacementRemove += RemovePlacement;
            KEvent_Building.OnPlacementRelocate += StartRelocatePlacement;

            // TODO Add a Cancal Logic For All Controller when you click esc it will close the lateest one with calling a methond in controller
        }

        public override void Dispose()
        {
            _view.OnSlotItemClicked -= StartATool;
            _view.OnStorageItemClicked -= StartInventoryItemPlacement;
            KEvent_Building.OnPlacementRemove -= RemovePlacement;
            KEvent_Building.OnPlacementRelocate -= StartRelocatePlacement;
        }

        public void Update(float deltaTime)
        {
            RequireIsInitialized();

            if (InputSystem.Instance.Esc)
            {
                if (currentTool != null)
                {
                    currentTool.OnStop(_toolHelper);
                    currentTool = null;
                }
            }

            if (currentTool == null) return;

            UpdateTool(_toolHelper);

            currentTool.OnUpdate(_toolHelper);

            if (currentTool.CheckPlaceInput(_toolHelper))
            {
                if (currentTool.OnValidate(_toolHelper))
                {
                    currentTool.OnPlace(_toolHelper);
                    KEvent_SoundFX.TriggerSoundFXPlay(SoundFXType.Success);
                }
                else
                {
                    KEvent_SoundFX.TriggerSoundFXPlay(SoundFXType.Error, true);
                }
            }
            
     
            if (InputSystem.Instance.CancelClick)
            {
                if(_toolHelper.Mode == PlacementMode.Relocating)
                    RelocateHandler(false);
                
                StopTool();
            }
            else if (currentTool.isFinished)
            {
                if(_toolHelper.Mode == PlacementMode.Relocating)
                    RelocateHandler(true);
                
                StopTool();
            }
        }

        private void UpdateTool(ToolHelper toolHelper)
        {
        }

        private void StartATool(StoreItemSO storeItemSo)
        {
            StopTool();

            _toolHelper.Mode = PlacementMode.Buying;
            ToolStartHandler(storeItemSo);
        }

        private void StartRelocatePlacement(int instanceID)
        {
            StopTool();

            _toolHelper.Mode = PlacementMode.Relocating;

            StoreItemSO item = _model.GetStoreItemByID(instanceID);

            Transform sceneObject = _model.GetPlacedSceneObjectByID(instanceID);
            sceneObject.gameObject.SetActive(false);

            if (sceneObject.TryGetComponent(out IPropUnit unit))
            {
                _toolHelper.SelectedPropItem = unit; 
                _toolHelper.startPosition = unit.WorldPos;
                _toolHelper.StartRotation = unit.transform.rotation;
            }
            else
            {
                Debug.LogError("Can't find Relocatable Scene Unit");
                return;
            }
            
            ToolStartHandler(item);
        }

        private void StartInventoryItemPlacement(StoreItemSO storeItemSo, int amount)
        {
            
        }
        
        private void ToolStartHandler(StoreItemSO storeItemSo)
        {
            _toolHelper.SelectedStoreItem = storeItemSo;

            currentTool = SelectBuildingMethod(storeItemSo);
            if (currentTool != null)
                currentTool.OnStart(_toolHelper);
        
            KEvent_Cursor.ChangeCursor(CursorSystem.eCursorTypes.Building);
            KEvent_Building.TriggerBuildingToggle(true);
        }

        private void RemovePlacement(int InstanceID)
        {
            Transform sceneObject = _model.GetPlacedSceneObjectByID(InstanceID);
            _model.RemovePlacementItem(InstanceID);
        
            if (sceneObject.TryGetComponent(out IPropUnit unit))
                KEvent_Building.TriggerPropRemoved(unit);
        
            UnityEngine.Object.Destroy(sceneObject.gameObject);
        }

        private void StopTool()
        {
            if (currentTool != null) currentTool.OnStop(_toolHelper);

            currentTool = null;

            _toolHelper.SelectedPropItem = null;
            _toolHelper.Mode = PlacementMode.None;
        
            KEvent_Cursor.ChangeToPrevious();
            KEvent_Building.TriggerBuildingToggle(false);
        }

        private void CancelTool()
        {
            StopTool();
        }

        private ITool SelectBuildingMethod(StoreItemSO storeItemSo)
        {
            if (storeItemSo is PlacementItemSO placement)
            {
                switch (placement.PlacementLayer)
                {
                    case ePlacementLayer.BaseSurface:
                        return new ISurfacePlacementTool();
                    case ePlacementLayer.FloorProp:
                        return new IFloorPlacementTool();
                    case ePlacementLayer.WallProp:
                        return new IWallPlacementTool();
                    case ePlacementLayer.Null:
                        return null;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (storeItemSo is MaterialItemSo material)
            {
                switch (material.MaterialLayer)
                {
                    case eMaterialLayer.FloorMaterial:
                        return new IFloorMaterialPlacerTool();
                    case eMaterialLayer.WallMaterial:
                        return new IWallMaterialPlacementTool();
                    case eMaterialLayer.Null:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }

        public void ToggleBuildingPage(bool toggle)
        {
            _view.ToggleView(toggle);
        }

        public void AddPlacementItemData(StoreItemSO itemSo, Transform sceneObject, Vector3 placedPosition, Quaternion placedRotation)
        {
            _model.AddPlacmeentItem(itemSo, sceneObject, placedPosition, placedRotation);
            KEvent_Building.TriggerPlaced();

            if (sceneObject.TryGetComponent(out IPropUnit unit))
            {
                KEvent_Building.TriggerPropPlaced(unit);
            }
        }

        private void RelocateHandler(bool isPlaced)
        {
            if (isPlaced)
            {
                if (_toolHelper.SelectedPropItem != null)
                {
                    _toolHelper.SelectedPropItem.SetPositionAndRotation(_toolHelper.LastPosition, _toolHelper.LastRotation);
                    _toolHelper.SelectedPropItem.gameObject.SetActive(true);
                    // _toolHelper.SelectedPropItem.OnRelocate();

                    _toolHelper.SelectedPropItem = null;
                    
                    KEvent_Building.TriggerPlaced();
                }
            }
            else
            {
                _toolHelper.SelectedPropItem.gameObject.SetActive(true);
                _toolHelper.SelectedPropItem = null;
            }
        }
    }
}