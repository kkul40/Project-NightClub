using Data;
using Disco_Building;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class RemoveButtonBase : UIButtonBase
    {
        [SerializeField] private ePlacementLayer removeLayer;
        public override void OnClick()
        {
            BuildingManager.Instance.StartRemoving(removeLayer);
        }
    }
}