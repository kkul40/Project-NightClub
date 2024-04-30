using UnityEngine;

public class Wall : MonoBehaviour
{
    public MeshRenderer MeshRenderer { get; protected set; }
    
    protected void Start()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
        GameData.Instance.WallMap.Add(this);
    }

    public virtual void ChangeMaterial(Material newWallPaper)
    {
        var materials = MeshRenderer.materials;
        materials[1] = newWallPaper;
        MeshRenderer.materials = materials;
    }

    public virtual void ResetMaterial(Material[] materials)
    {
        var currentMaterial = MeshRenderer.materials;
        currentMaterial = materials;
        MeshRenderer.materials = currentMaterial;
    }
}

