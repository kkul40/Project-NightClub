using UnityEngine;

namespace _Project.Script.NewSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "new Npc Animation Set")]
    public class NpcAnimationSo : ScriptableObject
    {
        [field : SerializeField]
        public AnimationClip Idle { get; private set; }
        [field : SerializeField]
        public AnimationClip Walk { get; private set; }
        [field : SerializeField]
        public AnimationClip Sit { get; private set; }
        [field : SerializeField]
        public AnimationClip Dance { get; private set; }
        [field : SerializeField]
        public AnimationClip Debug { get; private set; }
    }
}