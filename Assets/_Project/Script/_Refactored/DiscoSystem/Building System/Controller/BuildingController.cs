using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using DiscoSystem.Building_System.Controller.Tools;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Building_System.Model;
using DiscoSystem.Building_System.Service;
using DiscoSystem.Building_System.View;
using DiscoSystem.MusicPlayer;
using ExtensionMethods;
using Framework.Context;
using Framework.Mvcs.Controller;
using PropBehaviours;
using SaveAndLoad;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DiscoSystem.Building_System.Controller
{
    public enum GridSizes
    {
        FullGrid, // 1 x 1
        HalfGrid, // 0.5 x 0.5f
    }

    public class BuildingController : BaseController<BuildingModel, BuildingView, BuildingService>
    {
        // TODO: Remove This Relocate Data instead use tools to relocate props.
        private class RelocateData
        {
            public bool IsRelocating;
            public Transform SceneObject;
            public IPropUnit PropUnit;
            public Vector3 SavedPosition;
            public Quaternion SavedRotation;

            public List<Vector3Int> UpdatePoints;

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
                UpdatePoints = new List<Vector3Int>();
            }

            public RelocateData(IPropUnit propUnit, ToolHelper TH)
            {
                IsRelocating = true;
                SceneObject = propUnit.transform;
                PropUnit = propUnit;

                SavedPosition = SceneObject.position;
                SavedRotation = SceneObject.rotation;

                UpdatePoints = TH.GetPlacedPosition(SceneObject);

                SceneObject.gameObject.SetActive(false);
            }

            public void SetPosition(Vector3 newPosition, Quaternion newRotation, ToolHelper toolHelper)
            {
                SceneObject.rotation = newRotation;
                SceneObject.position = newPosition;

                List<Vector3Int> newPoints = toolHelper.GetPlacedPosition();

                foreach (var point in newPoints)
                    if (!UpdatePoints.Contains(point))
                        UpdatePoints.Add(point);
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
            DiscoData discoData, MaterialColorChanger materialColorChanger, FXCreatorSystem fxCreatorSystem, TileIndicator tileIndicator) : base(model, view,
            service)
        {
            _toolHelper = new ToolHelper(this, inputSystem, discoData, materialColorChanger, fxCreatorSystem, tileIndicator);
        }

        public override void Initialize(IContext context)
        {
            base.Initialize(context);

            _view.OnSlotItemClicked += StartATool;
            _view.OnStorageItemClicked += StartInventoryItemPlacement;
            _view.OnExtensionMapItemClicked += StartMapExtensionItem;
            
            _relocateData = new RelocateData();
            
            GameEvent.Subscribe<Event_RelocatePlacement>(StartRelocatePlacement);
            GameEvent.Subscribe<Event_RelocateWallDoor>(StartWallDoorRelocate);
            GameEvent.Subscribe<Event_RemovePlacement>(RemovePlacement);
            
            LoadItems();

            // TODO Add a Cancal Logic For All Controller when you click esc it will close the lateest one with calling a methond in controller
        }

        public void LoadItems()
        {
            List<Vector3Int> pathCoordinates = new List<Vector3Int>();
            foreach (var placementData in SaveLoadSystem.Instance.GetCurrentData().mapData.placementDatas)
            {
                PlacementItemSO item = GameBundle.Instance.FindAItemByID(placementData.PropID) as PlacementItemSO;
                Vector3 position = placementData.PlacedPosition;
                Quaternion rotation = Quaternion.Euler(placementData.EularAngles);
                var obj = DiscoData.Instantiate(item.Prefab, position, rotation);
                
                obj.GetComponent<IPropUnit>().Initialize(item.ID, item.PlacementLayer);

                _toolHelper.CalculateBounds(obj.GetComponents<Collider>());
                foreach (var pos in _toolHelper.GetPlacedPosition())
                    pathCoordinates.Add(pos);

                AddPlacementItemData(item, obj.transform, position, rotation, _toolHelper.colliderSize, null, false);
                obj.AnimatedPlacement(ePlacementAnimationType.Shaky);
            }
            GameEvent.Trigger(new Event_PropPlaced(null, pathCoordinates));
        }

        public void Update(float deltaTime)
        {
            RequireIsInitialized();

            if (_view.IsToggled && InputSystem.Instance.Undo(InputType.WasPressedThisFrame) && _currentTool == null)
                _toolHelper.PlacementTracker.Undo();
            
            if (_currentTool == null) return;
            
            if (InputSystem.Instance.GetEscape(InputType.WasPressedThisFrame))
            {
                RelocateHandler(false);
                StopBuilding();
                return;
            }

            _currentTool.OnUpdate(_toolHelper);

            bool cancelClick = InputSystem.Instance.GetCancel(InputType.WasPressedThisFrame) || _currentTool.isFinished;

            if (_currentTool.CheckPlaceInput(_toolHelper))
            {
                if (_relocateData.IsRelocating) // Relocate Handler
                {
                    if (_currentTool.OnValidate(_toolHelper))
                    {
                        RelocateHandler(true);
                        _currentTool.OnStop(_toolHelper);
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
                    if (_currentTool.OnValidate(_toolHelper) && TryPurchase(_toolHelper.PurchaseMode, _toolHelper.SelectedStoreItem))
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
                StopBuilding();
            }
        }
        
        private bool TryPurchase(PurchaseTypes purchaseTypes, StoreItemSO storeItemSo)
        {
            switch (purchaseTypes)
            {
                case PurchaseTypes.Buy:
                    if (DiscoData.Instance.inventory.HasEnoughMoney(storeItemSo.Price))
                    {
                        GameEvent.Trigger(new Event_RemoveMoney(storeItemSo.Price, false));
                        return true;
                    }
                    
                    Debug.Log("Not Enough Money To Buy");
                    // TODO : Send a notification window

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
                _relocateData = new RelocateData(unit, _toolHelper);
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
            
            GameEvent.Trigger(new Event_SelectCursor(CursorSystem.eCursorTypes.Building));
            GameEvent.Trigger(new Event_StartedPlacing());
            GameEvent.Trigger(new Event_ToggleBuildingMode(true));
        }

        private void StartMapExtensionItem(ExtendItemSo extendItemSo)
        {
            if (!DiscoData.Instance.MapData.CheckMapExpendable(extendItemSo.ExtendX, extendItemSo.ExtendY))
            {
                Debug.Log("You Cannot Expend The Map Further");
                // TODO : Send Notification Window
                return;
            }
            
            if (TryPurchase(PurchaseTypes.Buy, extendItemSo))
            {
                _toolHelper.PlacementTracker.AddTrack(new MapSizeUndo(extendItemSo.ID, new Vector2Int(extendItemSo.ExtendX, extendItemSo.ExtendY)));
                GameEvent.Trigger(new Event_ExpendMapSize(extendItemSo.ExtendX, extendItemSo.ExtendY));
            }
        }
        
        private void StartInventoryItemPlacement(StoreItemSO arg1, int arg2)
        {
            // TODO: Impelement This Later On!!!
            _toolHelper.PurchaseMode = PurchaseTypes.Free;
            throw new NotImplementedException();
        }

        private void StartBuilding(StoreItemSO storeItemSo)
        {
            _toolHelper.SelectedStoreItem = storeItemSo;

            _toolHelper.PurchaseMode = PurchaseTypes.Buy;

            _currentTool = SelectBuildingMethod(storeItemSo);
            if (_currentTool != null)
                _currentTool.OnStart(_toolHelper);

            GameEvent.Trigger(new Event_SelectCursor(CursorSystem.eCursorTypes.Building));
            GameEvent.Trigger(new Event_ResetCursorSelection());
            GameEvent.Trigger(new Event_StartedPlacing());
            GameEvent.Trigger(new Event_ToggleBuildingMode(true));
        }
        
        private void ClearBuildingCache()
        {
            if (_currentTool != null) _currentTool.OnStop(_toolHelper);

            _currentTool = null;
            _relocateData = new RelocateData();
            _toolHelper.SelectedStoreItem = null;
            _toolHelper.PurchaseMode = PurchaseTypes.None;
            _toolHelper.LastRotation = quaternion.identity;
            _toolHelper.KeepInStartPosition = false;
            _toolHelper.TileIndicator.CloseTileIndicator();
        }

        private void StopBuilding()
        {
            ClearBuildingCache();
            
            GameEvent.Trigger(new Event_ResetCursorIcon());
            GameEvent.Trigger(new Event_StoppedPlacing());
            
            if(!_view.IsToggled)
                GameEvent.Trigger(new Event_ToggleBuildingMode(false));
        }

        private void RemovePlacement(Event_RemovePlacement removeEvent)
        {
            Transform sceneObject = _model.GetPlacedSceneObjectByID(removeEvent.InstanceID);
            _model.RemovePlacementItem(removeEvent.InstanceID);
            
            if (sceneObject.TryGetComponent(out IPropUnit unit))
            {
                unit.IsInitialized = false;
                GameEvent.Trigger(new Event_PropRemoved(unit, _toolHelper.GetPlacedPosition(sceneObject)));
            }
            
            sceneObject.gameObject.AnimatedRemoval((() =>
            {
                Object.Destroy(sceneObject.gameObject);
            }));
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

        public void AddPlacementItemData(PlacementItemSO itemSo, Transform sceneObject, Vector3 placedPosition, Quaternion placedRotation, Vector3 colliderSize, List<Vector3Int> placedPoints = null, bool triggerEvent = true)
        {
            _model.AddPlacmeentItem(itemSo, sceneObject, placedPosition, placedRotation, colliderSize);
            sceneObject.SetParent(SceneGameObjectHandler.Instance.GetHolderByLayer(itemSo.PlacementLayer));

            if (triggerEvent)
            {
                if (sceneObject.TryGetComponent(out IPropUnit unit))
                    GameEvent.Trigger(new Event_PropPlaced(unit, placedPoints ?? new List<Vector3Int>()));
            }

            sceneObject.gameObject.AnimatedPlacement(ePlacementAnimationType.Shaky);
        }

        private void RelocateHandler(bool isVerified)
        {
            if (!_relocateData.IsRelocating) return;
            
            if (isVerified)
            {
                _relocateData.SetPosition(_toolHelper.LastPosition, _toolHelper.LastRotation, _toolHelper);
                _relocateData.ToggleGameObject(true);
                
                GameEvent.Trigger(new Event_PropRelocated(_relocateData.PropUnit, _relocateData.UpdatePoints));
            }
            else
            {
                _relocateData.ResetPosition();
                _relocateData.ToggleGameObject(true);
            }
        }
    }
}