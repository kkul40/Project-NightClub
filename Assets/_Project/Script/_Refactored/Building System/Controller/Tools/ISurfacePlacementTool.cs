using System.Collections.Generic;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;

public class ISurfacePlacementTool : ITool
{
    private GameObject _tempObject;
    private PlacementItemSO _placementItem;
    private List<MeshRenderer> _tempMeshRenderer;

    public bool isFinished { get; }
    public void OnStart(ToolHelper TH)
    {
        _placementItem = TH.SelectedStoreItem as PlacementItemSO;
        
        _tempObject = Object.Instantiate(_placementItem.Prefab, TH.LastPosition, TH.LastRotation);
        _tempObject.transform.SetParent(null);
        
        TH.CalculateBounds(_tempObject.GetComponents<Collider>());
        
        _tempMeshRenderer = TH.MaterialColorChanger.ReturnMeshRendererList(_tempObject);
    }

    public bool OnValidate(ToolHelper TH)
    {
        var gridData = TH.DiscoData.MapData.GetFloorGridData((int)TH.LastPosition.x, (int)TH.LastPosition.z);
        if (gridData == null)
            return false;
        
        var colliders = Physics.OverlapBox(TH.GetCenterOfBounds(),TH.colliderExtend * 0.98f, TH.LastRotation);
        for (int i = 0; i < colliders.Length; i++)
        {
            var hitObject = colliders[i];

            var hitUnit = hitObject.GetComponentInParent<IPropUnit>();
            if (hitUnit == null || hitUnit.transform == _tempObject.transform)
                continue;

            if (hitObject.TryGetComponent(out Wall wall))
                return false;

            IPropUnit propUnit = hitObject.GetComponentInParent<IPropUnit>();
           
            if (propUnit != null)
            {
                if (propUnit.PlacementLayer == _placementItem.PlacementLayer)
                    return false;
            }
        }
        return true;
    }

    public void OnUpdate(ToolHelper TH)
    {
        Vector3 mousePos = TH.InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID);
        
        bool validation = OnValidate(TH);

        var gridData = TH.DiscoData.MapData.GetFloorGridData((int)mousePos.x, (int)mousePos.z);
        
        Debug.Log(gridData);
        if (gridData != null)
        {
            TH.LastPosition = gridData.CellPosition.CellCenterPosition(eGridType.PlacementGrid);
        }
        
        TH.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, validation);

        Debug.Log(TH.LastPosition);
        _tempObject.transform.position = TH.LastPosition;
    }

    public void OnPlace(ToolHelper TH)
    {
        var obj = Object.Instantiate(_placementItem.Prefab, TH.LastPosition, TH.LastRotation);
        
        IPropUnit unit;
        if (obj.TryGetComponent(out IPropUnit propUnit))
            unit = propUnit;
        else
            unit = obj.AddComponent<IPropUnit>();

        unit.Initialize(_placementItem.ID, new Vector3Int((int)TH.LastPosition.x, (int)TH.LastPosition.y, (int)TH.LastPosition.z), RotationData.Default, ePlacementLayer.BaseSurface);
    }

    public void OnStop(ToolHelper TH)
    {
        if (_tempObject != null)
        {
            Object.Destroy(_tempObject.gameObject);
        }
    }
}