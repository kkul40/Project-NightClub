using UnityEngine;

public class Prop : MonoBehaviour
{
    private PropSo propSo;
    public Vector3 propPosition;

    public void Initialize(PropSo propSo, Vector3 propPos)
    {
        this.propSo = propSo;
        this.propPosition = propPos;
    }

    public Vector3 GetPropPositionVector3Int() => propPosition;
}
