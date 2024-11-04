using UnityEngine;

namespace BuildingSystem.Builders
{
    public class NullBuilderMethod : IBuildingMethod
    {
        public bool PressAndHold { get; }
        public bool isFinished { get; }

        public void OnStart(BuildingNeedsData BD)
        {
        }

        public bool OnValidate(BuildingNeedsData BD)
        {
            return false;
        }

        public void OnUpdate(BuildingNeedsData BD)
        {
        }

        public void OnPlace(BuildingNeedsData BD)
        {
        }

        public void OnStop(BuildingNeedsData BD)
        {
        }
    }
}