using UnityEngine;

namespace _Project.Script.NewSystem
{
    public class Table : Prop
    {
        public GameObject CubePrefab;

        public override void Initialize(PlacablePropSo placablePropSo, Vector3 propPos)
        {
            base.Initialize(placablePropSo, propPos);
        }
    }
}