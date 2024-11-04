using UnityEngine;

namespace BuildingSystem.Builders
{
    public interface IBuildingMethod
    {
        bool PressAndHold { get; }
        bool isFinished { get; }
        void OnStart(BuildingNeedsData BD);
        bool OnValidate(BuildingNeedsData BD);
        void OnUpdate(BuildingNeedsData BD);
        void OnPlace(BuildingNeedsData BD);
        void OnStop(BuildingNeedsData BD);
    }
}