using UnityEngine;

public class Wall : TileObject
{
    protected override void Start()
    {
        base.Start();
        GameData.Instance.WallMap.Add(this);
    }

    public override void ChangeMaterial(Material newWallPaper)
    {
        var materials = _meshRenderer.materials;
        materials[1] = newWallPaper;
        _meshRenderer.materials = materials;
    }
}

