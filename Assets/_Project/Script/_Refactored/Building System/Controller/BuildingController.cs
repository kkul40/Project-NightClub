using System;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using ExtensionMethods;
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
        _view.OnSlotItemClicked += OnSlotClickHandler;
    }

    public void Update(float deltaTime)
    {
        if (currentTool == null) return;

        UpdateTool(_toolHelper);
        
        currentTool.OnUpdate(_toolHelper);

        if (InputSystem.Instance.LeftClickOnWorld && currentTool.OnValidate(_toolHelper))
        {
            currentTool.OnPlace(_toolHelper);
            currentTool.OnStop(_toolHelper);
            currentTool = null;
        }
    }

    private void UpdateTool(ToolHelper toolHelper)
    {
        toolHelper.CellPosition = InputSystem.Instance.MousePosition.WorldPosToCellPos(eGridType.PlacementGrid);
    }

    private void OnSlotClickHandler(StoreItemSO storeItemSo)
    {
        _toolHelper.SelectedStoreItem = storeItemSo;
        
        currentTool = SelectBuildingMethod(storeItemSo);
        if(currentTool != null)
            currentTool.OnStart(_toolHelper);
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
