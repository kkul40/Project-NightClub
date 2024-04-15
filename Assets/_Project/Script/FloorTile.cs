using UnityEngine;

public class FloorTile : MonoBehaviour, IMaterial, IRemovable
{
    public MeshRenderer MeshRenderer { get; private set; }
    public FloorType floorType;
    
    private void Start()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
    }
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

    public void Remove()
    {
        throw new System.NotImplementedException();
    }
}

public enum FloorType
{
    Normal,
    Dance,
}