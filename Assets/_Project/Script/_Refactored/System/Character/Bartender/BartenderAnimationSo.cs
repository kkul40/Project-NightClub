using UnityEngine;

namespace System.Character.Bartender
{
    [CreateAssetMenu(fileName = "new Bartender Animation Set", menuName = "Data/Bartender Animation Set")]
    public class BartenderAnimationSo : ScriptableObject
    {
        [field: SerializeField] public float animationDuration { get; private set; }
        [field: SerializeField] public AnimationClip Idle { get; private set; }
        [field: SerializeField] public AnimationClip Walk { get; private set; }
        [field: SerializeField] public AnimationClip PrepareDrink { get; private set; }
        [field: SerializeField] public AnimationClip CleanUpTable { get; private set; }
        [field: SerializeField] public AnimationClip Debug { get; private set; }
    }
}