using BuildingSystem.Builders;
using BuildingSystem.SO;

namespace BuildingSystem
{
    public static class BuildingMethodFactory
    {
        public static IBuildingMethod GetBuildingMethod(StoreItemSO storeItemSo)
        {
            if (storeItemSo is PlacementItemSO placementItemSo)
            {
                return new PlacementBuilder();
            }

            if (storeItemSo is MaterialItemSo materialItemSo)
            {
                return new MaterialBuilder();
            }
            
            return null;
        }

        public static IRotationMethod GetRotationMethod(StoreItemSO storeItemSo)
        {
            if (storeItemSo is PlacementItemSO placementItemSo)
            {
                switch (placementItemSo.eRotation)
                {
                    case PlacementItemSO.eRotationType.None:
                        return null;
                    case PlacementItemSO.eRotationType.LeftRight:
                        return new RotationMethodLeftAndDown();
                    case PlacementItemSO.eRotationType.ThreeSixty:
                        return new RotationMethodler360();
                }
            }
            
            if (storeItemSo is MaterialItemSo materialItemSo)
            {
                return new RotationMethodHandlerAuto();
            }
            return null;
        }
    }
}