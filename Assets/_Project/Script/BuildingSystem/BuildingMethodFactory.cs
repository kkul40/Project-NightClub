using BuildingSystem.Builders;
using BuildingSystem.SO;
using Data;

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
            
            return new NullBuilderMethod();
        }

        public static IRotationMethod GetRotationMethod(StoreItemSO storeItemSo)
        {
            if (storeItemSo is PlacementItemSO placementItemSo)
            {
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
            }
            
            if (storeItemSo is MaterialItemSo materialItemSo)
            {
                return new RotationMethodHandlerAuto();
            }
            
            return new NullRotationMethod();
        }

        public static ePlacementLayer GetPlacementLayer(StoreItemSO storeItemSo)
        {
            if (storeItemSo is PlacementItemSO placementItemSo)
            {
                return placementItemSo.PlacementLayer;
            }

            return ePlacementLayer.Null;
        }

        public static eMaterialLayer GetMaterialLayer(StoreItemSO storeItemSo)
        {
            if (storeItemSo is MaterialItemSo materialItemSo)
            {
                return materialItemSo.MaterialLayer;
            }

            return eMaterialLayer.Null;
        }
    }
}