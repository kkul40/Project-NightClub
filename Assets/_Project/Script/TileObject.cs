using UnityEngine;

public abstract class TileObject : MonoBehaviour
{
    protected MeshRenderer _meshRenderer;

    protected virtual void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public virtual void ChangeMaterial(Material newWallPaper)
    {
        var materials = _meshRenderer.materials;
        materials[0] = newWallPaper;
        _meshRenderer.materials = materials;
    }

    public virtual void ResetMaterial(Material[] defaultMaterials)
    {
        var materials = _meshRenderer.materials;
        materials = defaultMaterials;
        _meshRenderer.materials = materials;
    }

    public Material[] GetCurrentMaterial()
    {
        return _meshRenderer.materials;
    }
}