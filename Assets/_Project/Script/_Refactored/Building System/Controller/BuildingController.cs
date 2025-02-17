using System;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using RMC.Mini;
using RMC.Mini.Controller;

public enum GridSizes
{
    FullGrid, // 1 x 1
    HalfGrid, // 0.5 x 0.5f
}

public class BuildingController : BaseController<BuildingModel, BuildingView, BuildingService>
{
    private ToolHelper _toolHelper;
    private ITool currentTool;
    
    public BuildingController(BuildingModel model, BuildingView view, BuildingService service, InputSystem inputSystem, DiscoData discoData, MaterialColorChanger materialColorChanger, FXCreator fxCreator) : base(model, view, service)
    {
        _toolHelper = new ToolHelper(inputSystem, discoData, materialColorChanger, fxCreator);
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

        // TODO Puth This InputSystem key fucntion somewhere that is not this controller
        if (InputSystem.Instance.LeftClickOnWorld && currentTool.OnValidate(_toolHelper))
        {
            currentTool.OnPlace(_toolHelper);
            if (currentTool.isFinished)
            {
                StopTool();
            }
        }
    }

    private void UpdateTool(ToolHelper toolHelper)
    {
    }

    private void StartATool(StoreItemSO storeItemSo)
    {
        StopTool();
        
        _toolHelper.SelectedStoreItem = storeItemSo;
        
        currentTool = SelectBuildingMethod(storeItemSo);
        if(currentTool != null)
            currentTool.OnStart(_toolHelper);
    }

    private void StopTool()
    {
        if(currentTool != null) currentTool.OnStop(_toolHelper);

        currentTool = null;
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
        return null;
    }
}
