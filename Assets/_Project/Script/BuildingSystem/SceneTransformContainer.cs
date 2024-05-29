using UnityEngine;

namespace BuildingSystem
{
    public class SceneTransformContainer : MonoBehaviour
    {
        [field: SerializeField] public Transform PropHolderTransform { get; private set; }
    }
}