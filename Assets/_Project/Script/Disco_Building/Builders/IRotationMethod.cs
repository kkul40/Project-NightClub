namespace Disco_Building.Builders
{
    public interface IRotationMethod
    {
        void OnStart(BuildingNeedsData buildingNeedsData);
        void OnRotate(BuildingNeedsData buildingNeedsData);
    }
}