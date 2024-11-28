using Disco_ScriptableObject;
using UI.GamePages;
using UI.StartMenu;
using UnityEngine;

namespace PropBehaviours
{
    public class WallDoor : Wall
    {
        [SerializeField] private Transform ChieldWallTransform;

        public override MeshRenderer _meshRenderer => ChieldWallTransform.GetComponent<MeshRenderer>();

        public override Material CurrentMaterial => _meshRenderer.materials[2];

        public override void UpdateMaterial(MaterialItemSo materialItemSo)
        {
            var materials = _meshRenderer.materials;
            materials[2] = materialItemSo.Material;
            _meshRenderer.materials = materials;
            assignedMaterialID = materialItemSo.ID;
        }

        #region Interactable
        public override eInteraction Interaction { get; } = eInteraction.PropUnit;

        public override void OnClick()
        {
            UIPageManager.Instance.RequestAPage(typeof(UIActionSelectionPage), this);
        }

        #endregion

    }
}