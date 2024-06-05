using System.Collections.Generic;
using BuildingSystem;
using Data;
using UnityEngine;

namespace PropBehaviours
{
    public class Table : Prop, IPropUpdate
    {
        public GameObject CubePrefab;
        public List<Chair> Chairs;

        public void PropUpdate()
        {
            Chairs.Clear();
            foreach (var prop in DiscoData.Instance.GetPropList)
            {
                if (Chairs.Count > 3) break;

                if (prop is Chair chair)
                {
                    var distance = chair.CellPosition - CellPosition;
                    if (distance.magnitude <= 1)
                    {
                        Chairs.Add(chair);
                    }
                }
            }
        }

        public override void Initialize(Vector3Int cellPosition, Direction direction)
        {
            base.Initialize(cellPosition, direction);

            PropUpdate();
        }
    }
}