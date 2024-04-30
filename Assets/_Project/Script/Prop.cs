using System;
using ScriptableObjects;
using UnityEngine;

[Serializable]
public class Prop : MonoBehaviour
{
    private PlacablePropSo _placablePropSo;
    public Vector3Int propPosition;
    public Direction direction;

    public virtual void Initialize(PlacablePropSo placablePropSo, Vector3Int propPos, Direction direction)
    {
        this._placablePropSo = placablePropSo;
        this.propPosition = propPos;
        this.direction = direction;
    }

    public Vector3Int GetPropCellPosition() => propPosition;
    public Quaternion GetPropRotation() => transform.rotation;
    public PlacablePropSo GetPropSo() => _placablePropSo;
}

