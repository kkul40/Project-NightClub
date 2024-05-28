using UnityEngine;

namespace _1BuildingSystemNew
{
    public interface IBuildingMethod
    {
        bool isFinished { get; }
        Vector3 Offset { get; }
        void OnStart(BuildingNeedsData buildingNeedsData);
        bool OnValidate(BuildingNeedsData buildingNeedsData);
        void OnUpdate(BuildingNeedsData buildingNeedsData);
        void OnPlace(BuildingNeedsData buildingNeedsData);
        void OnFinish(BuildingNeedsData buildingNeedsData);
    }
}