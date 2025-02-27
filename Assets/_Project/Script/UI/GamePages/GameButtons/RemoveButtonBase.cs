using Data;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class RemoveButtonBase : UIButtonBase
    {
        [SerializeField] private ePlacementLayer removeLayer;
        public override void OnClick()
        {
            // TODO Implement Removing Toll By Surface Layer            
            // BuildingManager.Instance.StartRemoving(removeLayer);
        }
    }
}