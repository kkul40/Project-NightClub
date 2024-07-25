using BuildingSystem.Builders;
using BuildingSystem.SO;
using Data;

namespace BuildingSystem
{
    public static class Builder
    {
        public static IBuildingMethod BuildToIBuilding(StoreItemSO storeItemSo)
        {
            if (storeItemSo is PlacementItemSO placementItemSo) return new PlacementBuilder();

            if (storeItemSo is MaterialItemSo materialItemSo) return new MaterialBuilder();

            return new NullBuilderMethod();
        }

        public static IRotationMethod BuildToIRotation(StoreItemSO storeItemSo)
        {
            if (storeItemSo is PlacementItemSO placementItemSo)
                switch (placementItemSo.eRotation)
                {
                    case PlacementItemSO.eRotationType.None:
                        return new NullRotationMethod();
                    case PlacementItemSO.eRotationType.LeftRight:
                        return new RotationMethodLeftAndDown();
                    case PlacementItemSO.eRotationType.ThreeSixty:
                        return new RotationMethodler360();
                    case PlacementItemSO.eRotationType.Auto:
                        return new RotationMethodHandlerAuto();
                }

            if (storeItemSo is MaterialItemSo materialItemSo) return new RotationMethodHandlerAuto();

            return new NullRotationMethod();
        }
    }
}