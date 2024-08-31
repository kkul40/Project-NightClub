using BuildingSystem;
using UnityEngine;

namespace UI
{
    public class RemoveButtonBase : UIButtonBase
    {
        public override void OnClick()
        {
            BuildingManager.Instance.StartRemoving();
        }
    }
}