using NPCBehaviour.PathFinder;

namespace NPCBehaviour
{
    public interface IUnit
    {
        public eGenderType GenderType { get; }
        public IAnimationController AnimationController { get; }
        public IPathFinder PathFinder { get; }
    }
}