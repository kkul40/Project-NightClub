using UnityEngine;

public class Prop : MonoBehaviour
{
    private PropSo propSo;
    public Vector3 propPosition;

    public void Initialize(PropSo propSo, Vector3 propPos)
    {
        this.propSo = propSo;
        this.propPosition = propPos;
        GameData.Instance.placedProps.Add(this);
    }

    public Vector3 GetPropPosition() => propPosition;
    public Quaternion GetPropRotation() => transform.rotation;
    public PropSo GetPropSo() => propSo;
}