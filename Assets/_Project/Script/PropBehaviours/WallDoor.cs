using UnityEngine;

namespace PropBehaviours
{
    public class WallDoor : Wall
    {
        [SerializeField] private Transform ChieldWallTransform;

        public override MeshRenderer _meshRenderer
        {
            get
            {
                return ChieldWallTransform.GetComponent<MeshRenderer>();
            }
        }

        public override Material CurrentMaterial
        {
            get
            {
                return _meshRenderer.materials[2];
            }
        }

        public override void UpdateMaterial(Material material)
        {
            var materials = _meshRenderer.materials;
            materials[2] = material; 
            _meshRenderer.materials = materials;
            
            Debug.Log("Wall Door Updpateddddd");
        }
    }
}