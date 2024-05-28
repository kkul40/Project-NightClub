using Data;
using DG.Tweening;
using UnityEngine;

public class WallDoor : Wall, IMaterial
{
    [SerializeField] private Transform ChieldWallTransform;
    [SerializeField] private Transform ChieldDoorTransform;

    protected void Start()
    {
        MeshRenderer = ChieldWallTransform.GetComponent<MeshRenderer>();
        GameData.Instance.WallMap.Add(this);
        ToggleDoor(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out NPC.NPC npc))
            ToggleDoor(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out NPC.NPC npc))
            ToggleDoor(false);
    }

    public override void ChangeMaterial(Material newWallPaper)
    {
        var materials = MeshRenderer.materials;
        materials[0] = newWallPaper;
        materials[1] = newWallPaper;
        MeshRenderer.materials = materials;
    }

    private void ToggleDoor(bool toggle)
    {
        if (toggle)
            ChieldDoorTransform.DORotate(new Vector3(0, 105, 0), 0.5f);
        else
            ChieldDoorTransform.DORotate(new Vector3(0, 180, 0), 0.5f);
    }
}