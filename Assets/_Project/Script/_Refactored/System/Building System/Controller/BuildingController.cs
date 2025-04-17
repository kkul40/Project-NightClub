using System.Building_System.Controller.Tools;
using System.Building_System.GameEvents;
using System.Building_System.Model;
using System.Building_System.Service;
using System.Building_System.View;
using Data;
using Disco_ScriptableObject;
using DiscoSystem;
using Framework.Context;
using Framework.Mvcs.Controller;
using PropBehaviours;
using Unity.Mathematics;
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
        private class RelocateData
        {
            public bool IsRelocating;
            public Transform SceneObject;
            public IPropUnit PropUnit;
            public Vector3 SavedPosition;
            public Quaternion SavedRotation;

            /// <summary>
            /// Null Constructor
            /// </summary>
            public RelocateData()
            {
                IsRelocating = false;
                SceneObject = null;
                PropUnit = null;
                SavedPosition = Vector3.zero;
                SavedRotation = quaternion.identity;
            }

            public RelocateData(IPropUnit propUnit)
            {
                IsRelocating = true;
                SceneObject = propUnit.transform;
                PropUnit = propUnit;

                SavedPosition = SceneObject.position;
                SavedRotation = SceneObject.rotation;

                SceneObject.gameObject.SetActive(false);
            }

            public void SetPosition(Vector3 newPosition, Quaternion newRotation)
            {
                SceneObject.rotation = newRotation;
                SceneObject.position = newPosition;
            }

            public void ResetPosition()
            {
                SceneObject.rotation = SavedRotation;
                SceneObject.position = SavedPosition;
            }

            public void ToggleGameObject(bool toggle)
            {
                SceneObject.gameObject.SetActive(toggle);
            }
        }
        
        private ToolHelper _toolHelper;
        private ITool _currentTool;

        private RelocateData _relocateData;

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

            _relocateData = new RelocateData();
            
            GameEvent.Subscribe<Event_RelocatePlacement>(StartRelocatePlacement);
            GameEvent.Subscribe<Event_RelocateWallDoor>(StartWallDoorRelocate);
            GameEvent.Subscribe<Event_RemovePlacement>(RemovePlacement);
            // TODO Add a Cancal Logic For All Controller when you click esc it will close the lateest one with calling a methond in controller
        }

        public void Update(float deltaTime)
        {
            RequireIsInitialized();

            if (_currentTool == null) return;
            
            if (InputSystem.Instance.Esc)
            {
                _currentTool.OnStop(_toolHelper);
                _currentTool = null;
                return;
            }

            _currentTool.OnUpdate(_toolHelper);

            bool cancelClick = InputSystem.Instance.CancelClick || _currentTool.isFinished;

            if (_currentTool.CheckPlaceInput(_toolHelper))
            {
                // if (_currentTool is IWallDoorPlacerTool) // Relocate Wall Door
                // {
                //     if (_currentTool.OnValidate(_toolHelper))
                //     {
                //         _currentTool.OnPlace(_toolHelper);
                //         ClearBuildingCache();
                //         StopBuilding();
                //         GameEvent.Trigger(new Event_Sfx(SoundFXType.BuildingSuccess));
                //     }
                //     else
                //     {
                //         GameEvent.Trigger(new Event_Sfx(SoundFXType.BuildingError, true));
                //     }
                // }
                if (_relocateData.IsRelocating) // Relocate Handler
                {
                    if (_currentTool.OnValidate(_toolHelper))
                    {
                        RelocateHandler(true);
                        _currentTool.OnStop(_toolHelper);
                        ClearBuildingCache();
                        StopBuilding();
                        GameEvent.Trigger(new Event_Sfx(SoundFXType.BuildingSuccess));
                    }
                    else
                    {
                        GameEvent.Trigger(new Event_Sfx(SoundFXType.BuildingError, true));
                    }
                }
                else // Place Handler
                {
                    if (_currentTool.OnValidate(_toolHelper) && TryPurchase(_toolHelper))
                    {
                        _currentTool.OnPlace(_toolHelper);
                        GameEvent.Trigger(new Event_Sfx(SoundFXType.BuildingSuccess));
                    }
                    else
                    {
                        GameEvent.Trigger(new Event_Sfx(SoundFXType.BuildingError, true));
                    }
                }
            }
            
            if (cancelClick)
            {
                RelocateHandler(false);
                ClearBuildingCache();
                StopBuilding();
            }
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
                case PurchaseTypes.Unique:
                    return true;
            }

            return true;
        }

        
        private void StartATool(StoreItemSO storeItemSo)
        {
            ClearBuildingCache();

            _toolHelper.PurchaseMode = PurchaseTypes.Buy;
            StartBuilding(storeItemSo);
        }

        private void StartRelocatePlacement(Event_RelocatePlacement relocateEvent)
        {
            ClearBuildingCache();

            StoreItemSO item = _model.GetStoreItemByID(relocateEvent.InstanceID);
            Transform sceneObject = _model.GetPlacedSceneObjectByID(relocateEvent.InstanceID);

            if (sceneObject.TryGetComponent(out IPropUnit unit))
            {
                _relocateData = new RelocateData(unit);
                _toolHelper.KeepInStartPosition = true;
                _toolHelper.StartMousePos = _toolHelper.InputSystem.MousePosition;
                _toolHelper.LastPosition = _relocateData.SavedPosition;
                _toolHelper.LastRotation = _relocateData.SavedRotation;
                _relocateData.ToggleGameObject(false);
            }
            else
            {
                Debug.LogError("Can't find Relocatable Scene Unit");
                return;
            }
            
            StartBuilding(item);
        }

        private void StartWallDoorRelocate(Event_RelocateWallDoor relocateWallEvent)
        {
            ClearBuildingCache();

            _toolHelper.PurchaseMode = PurchaseTypes.Free;
            
            _currentTool = new IWallDoorPlacerTool();
            _currentTool.OnStart(_toolHelper);
            
            GameEvent.Trigger(new Event_ToggleBuildingMode(true));
        }
        
        private void StartInventoryItemPlacement(StoreItemSO arg1, int arg2)
        {
            _toolHelper.PurchaseMode = PurchaseTypes.Free;
            throw new NotImplementedException();
        }

        private void StartBuilding(StoreItemSO storeItemSo)
        {
            _toolHelper.SelectedStoreItem = storeItemSo;

            _currentTool = SelectBuildingMethod(storeItemSo);
            if (_currentTool != null)
                _currentTool.OnStart(_toolHelper);
        
            GameEvent.Trigger(new Event_SelectCursor(CursorSystem.eCursorTypes.Building));
            GameEvent.Trigger(new Event_ToggleBuildingMode(true));
        }
        
        private void ClearBuildingCache()
        {
            if (_currentTool != null) _currentTool.OnStop(_toolHelper);

            _currentTool = null;
            _relocateData = new RelocateData();
            _toolHelper.PurchaseMode = PurchaseTypes.None;
            _toolHelper.LastRotation = quaternion.identity;
            _toolHelper.KeepInStartPosition = false;
        }

        private void StopBuilding()
        {
            ClearBuildingCache();
            
            GameEvent.Trigger(new Event_PreviousCursor());
            GameEvent.Trigger(new Event_ToggleBuildingMode(false));
        }

        private void RemovePlacement(Event_RemovePlacement removeEvent)
        {
            Transform sceneObject = _model.GetPlacedSceneObjectByID(removeEvent.InstanceID);
            _model.RemovePlacementItem(removeEvent.InstanceID);
            
            sceneObject.gameObject.SetActive(false);
            if (sceneObject.TryGetComponent(out IPropUnit unit))
                GameEvent.Trigger(new Event_PropRemoved(unit));
        
            UnityEngine.Object.Destroy(sceneObject.gameObject);
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

        private void RelocateHandler(bool isVerified)
        {
            if (!_relocateData.IsRelocating) return;
            

            if (isVerified)
            {
                _relocateData.SetPosition(_toolHelper.LastPosition, _toolHelper.LastRotation);
                _relocateData.ToggleGameObject(true);
                GameEvent.Trigger(new Event_PropRelocated(_relocateData.PropUnit));
            }
            else
            {
                _relocateData.ResetPosition();
                _relocateData.ToggleGameObject(true);
            }
        }
    }
}