using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "new Npc Animation Set")]
    public class NpcAnimationSo : ScriptableObject
    {
        [field: SerializeField] public float animationDuration { get; private set; }

        [field: SerializeField] public List<AnimationClip> Idle { get; private set; }

        [field: SerializeField] public List<AnimationClip> Walk { get; private set; }

        [field: SerializeField] public List<AnimationClip> Sit { get; private set; }

        [field: SerializeField] public List<AnimationClip> Dance { get; private set; }

        [field: SerializeField] public List<AnimationClip> Debug { get; private set; }
    }
}