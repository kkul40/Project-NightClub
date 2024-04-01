using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class Table : Prop
{
    public GameObject CubePrefab;
    public List<Chair> Chairs;

    public override void Initialize(PlacablePropSo placablePropSo, Vector3Int propPos, Direction direction)
    {
        base.Initialize(placablePropSo, propPos, direction);

        foreach (var prop in GameData.Instance.GetPropList())
        {
            if (Chairs.Count > 4) break;
            
            if (prop is Chair chair)
            {
                var t = chair.propPosition - propPosition;
                if (t.magnitude <= 1)
                {
                    Chairs.Add(chair);
                }
            }
        }
    }
}