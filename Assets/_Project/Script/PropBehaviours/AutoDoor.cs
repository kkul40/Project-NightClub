using DG.Tweening;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [SerializeField] private Transform ChieldDoorTransform;

    private void Start()
    {
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

    private void ToggleDoor(bool toggle)
    {
        if (toggle)
            ChieldDoorTransform.DORotate(new Vector3(0, 105, 0), 0.5f);
        else
            ChieldDoorTransform.DORotate(new Vector3(0, 180, 0), 0.5f);
    }
}