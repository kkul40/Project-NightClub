using UnityEngine;

namespace PropBehaviours
{
    public class WallDoor : Wall
    {
        [SerializeField] private Transform ChieldWallTransform;

        public override void UpdateMaterial()
        {
            MeshRenderer meshRenderer = ChieldWallTransform.GetComponent<MeshRenderer>();
            var materials = meshRenderer.materials;
            materials[0] = CurrentMaterial; 
            meshRenderer.materials = materials;
        }
    }
}