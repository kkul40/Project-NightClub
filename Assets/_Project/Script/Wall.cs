using Data;
using UnityEngine;

public class Wall : MonoBehaviour, IMaterial, IInteractable
{
    protected void Start()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
        GameData.Instance.WallMap.Add(this);
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
    }

    public MeshRenderer MeshRenderer { get; protected set; }

    public virtual void ChangeMaterial(Material newWallPaper)
    {
        Debug.Log("Changed Material");
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