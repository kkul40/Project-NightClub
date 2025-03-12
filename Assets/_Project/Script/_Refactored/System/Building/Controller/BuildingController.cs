using System.Building.Controller.Tools;
using System.Building.Model;
using System.Building.Service;
using System.Building.View;
using System.Music;
using Data;
using Disco_ScriptableObject;
using Framework.Context;
using Framework.Mvcs.Controller;
using GameEvents;
using PropBehaviours;
using UnityEngine;

namespace System.Building.Controller
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

            _view.OnSlotItemClicked += StartATool;
            _view.OnStorageItemClicked += StartInventoryItemPlacement;
            
            GameEvent.Subscribe<Event_RelocatePlacement>(StartRelocatePlacement);
            GameEvent.Subscribe<Event_RelocateWallDoor>(StartWallDoorRelocate);
            GameEvent.Subscribe<Event_RemovePlacement>(RemovePlacement);

            // TODO Add a Cancal Logic For All Controller when you click esc it will close the lateest one with calling a methond in controller
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

            currentTool.OnUpdate(_toolHelper);

            if (currentTool.CheckPlaceInput(_toolHelper))
            {
                if (currentTool.OnValidate(_toolHelper) && TryPurchase(_toolHelper))
                {
                    currentTool.OnPlace(_toolHelper);
                    GameEvent.Trigger(new Event_Sfx(SoundFXType.BuildingSuccess, true));
                }
                else
                {
                    GameEvent.Trigger(new Event_Sfx(SoundFXType.BuildingError, true));
                }
            }
     
            if (InputSystem.Instance.CancelClick || currentTool.isFinished)
            {
                StopTool();
            }
        }
        
        private bool HasMoney(ToolHelper toolHelper)
        {
            if(toolHelper.PurchaseMode == PurchaseTypes.Buy && toolHelper.SelectedStoreItem != null)
                return DiscoData.Instance.inventory.HasEnoughMoney(_toolHelper.SelectedStoreItem.Price);

            return true;
        }

        private bool TryPurchase(ToolHelper toolHelper)
        {
            switch (toolHelper.PurchaseMode)
            {
                case PurchaseTypes.Buy:
                    if (DiscoData.Instance.inventory.HasEnoughMoney(_toolHelper.SelectedStoreItem.Price))
                    {
                        GameEvent.Trigger(new Event_RemoveMoney(_toolHelper.SelectedStoreItem.Price, false));
                        return true;
                    }

                    return false;
                case PurchaseTypes.Free:
                    return true;
                case PurchaseTypes.Sell:
                    return true;
                case PurchaseTypes.Unique:
                    return true;
            }

            return true;
        }


        private void StartATool(StoreItemSO storeItemSo)
        {
            StopTool();

            _toolHelper.PurchaseMode = PurchaseTypes.Buy;
            StartPlacementTool(storeItemSo);
        }

        private void StartRelocatePlacement(Event_RelocatePlacement relocateEvent)
        {
            StopTool();

            _toolHelper.PurchaseMode = PurchaseTypes.Free;
            _toolHelper.IsRelocating = true;

            StoreItemSO item = _model.GetStoreItemByID(relocateEvent.InstanceID);

            Transform sceneObject = _model.GetPlacedSceneObjectByID(relocateEvent.InstanceID);
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
            
            StartPlacementTool(item);
        }

        private void StartWallDoorRelocate(Event_RelocateWallDoor relocateWallEvent)
        {
            throw new NotImplementedException();

            StopTool();

            _toolHelper.IsRelocating = true;
            _toolHelper.PurchaseMode = PurchaseTypes.Free;
            
            currentTool = new IWallDoorPlacerTool();
            currentTool.OnStart(_toolHelper);
            
            GameEvent.Trigger(new Event_ToggleBuildingMode(true));
        }
        
        private void StartInventoryItemPlacement(StoreItemSO arg1, int arg2)
        {
            _toolHelper.PurchaseMode = PurchaseTypes.Free;
            throw new NotImplementedException();
        }

        private void StartPlacementTool(StoreItemSO storeItemSo)
        {
            _toolHelper.SelectedStoreItem = storeItemSo;

            currentTool = SelectBuildingMethod(storeItemSo);
            if (currentTool != null)
                currentTool.OnStart(_toolHelper);
        
            GameEvent.Trigger(new Event_SelectCursor(eCursorTypes.Building));
            GameEvent.Trigger(new Event_ToggleBuildingMode(true));
        }

        private void RemovePlacement(Event_RemovePlacement removeEvent)
        {
            Transform sceneObject = _model.GetPlacedSceneObjectByID(removeEvent.InstanceID);
            _model.RemovePlacementItem(removeEvent.InstanceID);
        
            if (sceneObject.TryGetComponent(out IPropUnit unit))
                GameEvent.Trigger(new Event_PropRemoved(unit));
        
            UnityEngine.Object.Destroy(sceneObject.gameObject);
        }

        private void StopTool()
        {
            if (currentTool != null) currentTool.OnStop(_toolHelper);

            currentTool = null;

            _toolHelper.SelectedPropItem = null;
            _toolHelper.IsRelocating = false;
            _toolHelper.PurchaseMode = PurchaseTypes.None;
        
            GameEvent.Trigger(new Event_PreviousCursor());
            GameEvent.Trigger(new Event_ToggleBuildingMode(false));
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

        public void AddPlacementItemData(StoreItemSO itemSo, Transform sceneObject, Vector3 placedPosition, Quaternion placedRotation)
        {
            _model.AddPlacmeentItem(itemSo, sceneObject, placedPosition, placedRotation);

            if (sceneObject.TryGetComponent(out IPropUnit unit))
            {
                GameEvent.Trigger(new Event_PropPlaced(unit));
            }
        }

        public void RelocateHandler(bool isPlaced)
        {
            if (isPlaced)
            {
                if (_toolHelper.SelectedPropItem != null)
                {
                    _toolHelper.SelectedPropItem.SetPositionAndRotation(_toolHelper.LastPosition, _toolHelper.LastRotation);
                    _toolHelper.SelectedPropItem.gameObject.SetActive(true);
                    // _toolHelper.SelectedPropItem.OnRelocate();

                    _toolHelper.SelectedPropItem = null;
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