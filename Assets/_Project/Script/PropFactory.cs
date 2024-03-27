using UnityEngine;

[DisallowMultipleComponent]
public class PropFactory : MonoBehaviour
{
    public static PropFactory Instance;

    private void Awake()
    {
        Instance = this;
    }
}