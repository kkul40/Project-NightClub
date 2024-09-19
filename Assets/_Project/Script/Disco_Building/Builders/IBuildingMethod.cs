using UnityEngine;

namespace BuildingSystem.Builders
{
    public interface IBuildingMethod
    {
        bool PressAndHold { get; }
        bool isFinished { get; }
        void OnStart(BuildingNeedsData buildingNeedsData);
        bool OnValidate(BuildingNeedsData buildingNeedsData);
        void OnUpdate(BuildingNeedsData buildingNeedsData);
        void OnPlace(BuildingNeedsData buildingNeedsData);
        void OnStop(BuildingNeedsData buildingNeedsData);
    }
}