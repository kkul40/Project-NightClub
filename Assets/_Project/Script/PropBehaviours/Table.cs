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
            print("update");
            Chairs.Clear();
            foreach (var prop in GameData.Instance.GetPropList)
            {
                if (Chairs.Count > 3) break;

                if (prop is Chair chair)
                {
                    print("Chair found");
                    var distance = chair.CellPosition - CellPosition;
                    print(distance.magnitude);
                    if (distance.magnitude <= 1)
                    {
                        Chairs.Add(chair);
                        print("Table Updated");
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