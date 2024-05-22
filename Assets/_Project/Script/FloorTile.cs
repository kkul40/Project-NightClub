using UnityEngine;

public class FloorTile : MonoBehaviour, IMaterial, IInteractable
{
    private void Start()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
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

    public MeshRenderer MeshRenderer { get; private set; }

    public void ChangeMaterial(Material material)
    {
        var materials = MeshRenderer.materials;
        materials[0] = material;
        MeshRenderer.materials = materials;
    }

    public void ResetMaterial(Material[] materials)
    {
        var currentMaterials = MeshRenderer.materials;
        currentMaterials = materials;
        MeshRenderer.materials = currentMaterials;
    }
}