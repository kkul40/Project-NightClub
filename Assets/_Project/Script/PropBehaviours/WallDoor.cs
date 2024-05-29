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
            materials[2] = CurrentMaterial; 
            meshRenderer.materials = materials;
        }
    }
}