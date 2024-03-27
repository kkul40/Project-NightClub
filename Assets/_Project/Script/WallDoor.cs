using System;
using DG.Tweening;
using UnityEngine;

public class WallDoor : Wall
{
    [SerializeField] private Transform ChieldWallTransform;
    [SerializeField] private Transform ChieldDoorTransform;
        
    protected override void Start()
    {
        GameData.Instance.WallMap.Add(this);
        _meshRenderer = ChieldWallTransform.GetComponent<MeshRenderer>();
        
        ToggleDoor(false);
    }

    public override void ChangeWallpaper(Material newWallPaper)
    {
        var materials = _meshRenderer.materials;
        materials[0] = newWallPaper;
        materials[1] = newWallPaper;
        _meshRenderer.materials = materials;
    }

    private void ToggleDoor(bool toggle)
    {
        if (toggle)
            ChieldDoorTransform.DORotate(new Vector3(0, 105, 0), 0.5f);
        else
            ChieldDoorTransform.DORotate(new Vector3(0, 180, 0), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Door OPen");
        if(other.transform.TryGetComponent(out NPC npc))
            ToggleDoor(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Door Close");

        if(other.transform.TryGetComponent(out NPC npc))
            ToggleDoor(false);
    }
}