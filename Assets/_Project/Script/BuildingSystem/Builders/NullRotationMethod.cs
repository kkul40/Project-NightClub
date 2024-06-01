namespace BuildingSystem.Builders
{
    public class NullRotationMethod : IRotationMethod
    {
        public void Rotate(BuildingNeedsData buildingNeedsData)
        {
            buildingNeedsData.RotationData = new RotationData();
        }
    }
}