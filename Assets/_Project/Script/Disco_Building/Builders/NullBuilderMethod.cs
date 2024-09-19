using UnityEngine;

namespace BuildingSystem.Builders
{
    public class NullBuilderMethod : IBuildingMethod
    {
        public bool PressAndHold { get; }
        public bool isFinished { get; }

        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
        }

        public bool OnValidate(BuildingNeedsData buildingNeedsData)
        {
            return false;
        }

        public void OnUpdate(BuildingNeedsData buildingNeedsData)
        {
        }

        public void OnPlace(BuildingNeedsData buildingNeedsData)
        {
        }

        public void OnStop(BuildingNeedsData buildingNeedsData)
        {
        }
    }
}