using Disco_ScriptableObject;
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
    }
}