using BuildingSystem;
using UnityEngine;

namespace UI
{
    public class RemoveButton : UIButton
    {
        public override void OnClick()
        {
            BuildingManager.Instance.StartRemoving();
        }
    }
}