using System;
using Data;
using DefaultNamespace._Refactored.Event;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using RMC.Mini;
using RMC.Mini.Controller;
using UnityEngine;
using Object = UnityEngine.Object;

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

        _view.InstantiateItems(_model.StoreItems);
        _view.OnSlotItemClicked += StartATool;

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

        UpdateTool(_toolHelper);

        currentTool.OnUpdate(_toolHelper);
        
        if (InputSystem.Instance.CancelClick || currentTool.isFinished)
        {
            StopTool();
        }

        // // TODO Puth This InputSystem key fucntion somewhere that is not this controller
        // if (InputSystem.Instance.LeftClickOnWorld && currentTool.OnValidate(_toolHelper))
        // {
        //     currentTool.OnPlace(_toolHelper);
        //     if (currentTool.isFinished)
        //     {
        //     }
        // }
    }

    private void UpdateTool(ToolHelper toolHelper)
    {
    }

    private void StartATool(StoreItemSO storeItemSo)
    {
        StopTool();

        _toolHelper.SelectedStoreItem = storeItemSo;

        currentTool = SelectBuildingMethod(storeItemSo);
        if (currentTool != null)
            currentTool.OnStart(_toolHelper);
        
        KEvent_Cursor.ChangeCursor(CursorSystem.eCursorTypes.Building);
        KEvent_Building.TriggerBuildingToggle(true);
    }

    private void StopTool()
    {
        if (currentTool != null) currentTool.OnStop(_toolHelper);

        currentTool = null;
        
        KEvent_Cursor.ChangeToPrevious();
        KEvent_Building.TriggerBuildingToggle(false);
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

    public void AddPlacementItemData(StoreItemSO itemSo, Transform sceneObject, Vector3 placedPosition,
        Quaternion placedRotation)
    {
        _model.PlacedItems.Add(sceneObject.GetInstanceID(), new Tuple<int, Transform, Vector3, Quaternion>(itemSo.ID, sceneObject, placedPosition, placedRotation));
    }

    public void RemovePlacementItemData(int InstanceID)
    {
        Transform sceneObject = _model.PlacedItems[InstanceID].Item2;
        Object.Destroy(sceneObject.gameObject);
        _model.PlacedItems.Remove(InstanceID);
    }
}
