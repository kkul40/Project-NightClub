using UnityEngine;

namespace UI
{
    public class UIPopUpBase : UIView
    {
        private Vector3 worldPosition;
        
        public virtual void OnShow(Vector3 worldPosition)
        {
            worldPosition = worldPosition;
        }
        
        public virtual void UpdatePosition()
        {
            // TODO Update Position
        }
    }
}