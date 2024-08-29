using UnityEngine;

namespace HighlightPlus.Demos
{
    public class ManualSelectionDemo : MonoBehaviour
    {
        public Transform objectToSelect;
        private HighlightManager hm;

        private void Start()
        {
            hm = Misc.FindObjectOfType<HighlightManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) hm.SelectObject(objectToSelect);

            if (Input.GetKeyDown(KeyCode.Alpha2)) hm.ToggleObject(objectToSelect);

            if (Input.GetKeyDown(KeyCode.Alpha3)) hm.UnselectObject(objectToSelect);
        }
    }
}