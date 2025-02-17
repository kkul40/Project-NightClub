using System.Collections.Generic;
using Disco_Building;
using Disco_ScriptableObject;
using UnityEngine;

public class IWallMaterialPlacementTool : ITool
{
    private MaterialItemSo _materialItemSo;
    
    private IChangableMaterial _mouseOnChangableMaterial;
    private IChangableMaterial _currentChangableMaterial;
    private MaterialItemSo _storedMaterial;
    
    private Quaternion _wallRotation;
    
    private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

    public bool isFinished { get; private set; }
    public void OnStart(ToolHelper TH)
    {
        _materialDatas = new Dictionary<Transform, MaterialColorChanger.MaterialData>();
        _materialItemSo = TH.SelectedStoreItem as MaterialItemSo;
    }

    public bool OnValidate(ToolHelper TH)
    {
        if (!TH.MouseInBoundryCheck()) return false;
        return true;
    }

    public void OnUpdate(ToolHelper TH)
    {
        
        Debug.Log(TH.InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID));
        _mouseOnChangableMaterial = GetClosestWallMaterial(TH);
        
        if (_mouseOnChangableMaterial == null || !TH.MouseInBoundryCheck())
        {
            ResetPreviousMaterial();
            return;
        }

        if (_mouseOnChangableMaterial != _currentChangableMaterial)
        {
            ResetPreviousMaterial();
            _currentChangableMaterial = _mouseOnChangableMaterial;

            _storedMaterial = TH.DiscoData.FindAItemByID(_currentChangableMaterial.assignedMaterialID) as MaterialItemSo;
            _mouseOnChangableMaterial.UpdateMaterial(_materialItemSo);
            Debug.Log("Material Changed");
        }
    }

    public void OnPlace(ToolHelper TH)
    {
    }

    public void OnStop(ToolHelper TH)
    {
        ResetPreviousMaterial();
        TH.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
    }
    
    private void ResetPreviousMaterial()
    {
        if (_currentChangableMaterial == null) return;
        _currentChangableMaterial.UpdateMaterial(_storedMaterial);
        _currentChangableMaterial = null;
    }
    
    private IChangableMaterial GetClosestWallMaterial(ToolHelper TH)
    {
        float lastDis = float.MaxValue;
        IChangableMaterial closestChangableMaterial = null;
        foreach (var wall in TH.DiscoData.MapData.WallDatas)
        {
            if (wall.assignedWall == null) continue;

            var dis = Vector3.Distance(TH.InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID), wall.assignedWall.transform.position);
            if (dis < lastDis)
            {
                closestChangableMaterial = wall.assignedWall;
                lastDis = dis;
            }
        }

        return closestChangableMaterial;
    }
}