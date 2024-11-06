using System;
using DG.Tweening;
using NPC;
using UnityEngine;

namespace PropBehaviours
{
    public class AutoDoor : MonoBehaviour
    {
        [SerializeField] private Transform ChieldDoorTransform;

        private float timer;

        private void Start()
        {
            ToggleDoor(false);
        }

        private void Update()
        {
            if (timer > 1)
            {
                ToggleDoor(false);
                timer = 3;
            }

            if (timer < 3)
                timer += Time.deltaTime;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.TryGetComponent(out IDoorOpener opener))
            {
                ToggleDoor(true);
                timer = 0;
            }
        }

        private void ToggleDoor(bool toggle)
        {
            if (toggle)
                ChieldDoorTransform.DORotate(new Vector3(0, 105, 0), 0.5f);
            else
                ChieldDoorTransform.DORotate(new Vector3(0, 180, 0), 0.5f);
        }
    }
}