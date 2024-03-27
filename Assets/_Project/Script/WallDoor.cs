using UnityEngine;

public class WallDoor : Wall
{
    [SerializeField] private Transform ChieldWallTransform;
        
    protected override void Start()
    {
        GameData.Instance.WallMap.Add(this);
        _meshRenderer = ChieldWallTransform.GetComponent<MeshRenderer>();
    }

    public override void ChangeWallpaper(Material newWallPaper)
    {
        var materials = _meshRenderer.materials;
        materials[0] = newWallPaper;
        materials[1] = newWallPaper;
        _meshRenderer.materials = materials;
    }
}