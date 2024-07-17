using UnityEngine;

namespace HighlightPlus.Demos
{
    public class HitFxDemo : MonoBehaviour
    {
        public AudioClip hitSound;

        private void Update()
        {
            if (!InputProxy.GetMouseButtonDown(0)) return;

            var ray = Camera.main.ScreenPointToRay(InputProxy.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                var effect = hitInfo.collider.GetComponent<HighlightEffect>();
                if (effect == null) return;
                AudioSource.PlayClipAtPoint(hitSound, hitInfo.point);
                effect.HitFX(hitInfo.point);
            }
        }
    }
}