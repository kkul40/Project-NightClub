using System;
using _1BuildingSystemNew;
using UnityEngine;

public class FloorTile : MonoBehaviour, IInteractable, IChangableMaterial
{
    private void Start()
    {
        LoadMaterial();
    }
    
    public eMaterialLayer MaterialLayer { get; } = eMaterialLayer.FloorTile;
    public Material CurrentMaterial { get; set; }

    public void LoadMaterial()
    {
        CurrentMaterial = InitConfig.Instance.GetDefaultTileMaterial.Material;
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material = CurrentMaterial;
    }

    public eInteraction Interaction { get; } = eInteraction.None;

    public void OnFocus()
    {
    }

    public void OnOutFocus()
    {
    }

    public void OnClick()
    {
        Debug.Log("Tile Floor");
    }
}