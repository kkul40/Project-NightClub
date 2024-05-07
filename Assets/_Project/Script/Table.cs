using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class Table : Prop, IPropUpdate
{
    public GameObject CubePrefab;
    public List<Chair> Chairs;

    public override void Initialize(PlacablePropSo placablePropSo, Vector3Int cellPosition, Direction direction)
    {
        base.Initialize(placablePropSo, cellPosition, direction);
        
        PropUpdate();
    }

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
}