using System;
using DG.Tweening;
using NPCBehaviour;
using UnityEngine;

namespace PropBehaviours
{
    public class AutoDoor : MonoBehaviour
    {
        [SerializeField] private Transform ChieldDoorTransform;

        private float timer;
        private bool isToggled = true;
        public bool Locked;

        private void Start()
        {
            ToggleDoor(false);
        }

        private void Update()
        {
            if (Locked)
            {
                ToggleDoor(true);
                return;
            }
            
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
            if (other.transform.TryGetComponent(out IDoorTrigger opener))
            {
                if (!opener.TriggerDoor) return;
                
                ToggleDoor(true);
                timer = 0;
            }
        }

        private void ToggleDoor(bool toggle)
        {
            if (isToggled == toggle) return;

            isToggled = toggle;

            float targetYRotation = toggle ? 90f : 180f;

            ChieldDoorTransform.DOLocalRotate(new Vector3(0, targetYRotation, 0), 0.5f);
        }
    }
}