using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Prop : MonoBehaviour
{
    private PlacablePropSo _placablePropSo;
    public Vector3Int CellPosition;
    public Direction direction;

    public virtual void Initialize(PlacablePropSo placablePropSo, Vector3Int cellPosition, Direction direction)
    {
        this._placablePropSo = placablePropSo;
        this.CellPosition = cellPosition;
        this.direction = direction;
    }

    public Vector3Int GetPropCellPosition() => CellPosition;
    public Quaternion GetPropRotation() => transform.rotation;
    public PlacablePropSo GetPropSo() => _placablePropSo;
}

