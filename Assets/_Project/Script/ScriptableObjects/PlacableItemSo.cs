using BuildingSystemFolder;
using UnityEngine;              

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placeable/New Item")]
    public class PlacableItemDataSo : ScriptableObject, IPlaceableItemData
    {
        public Sprite icon;
        public GameObject prefab;
        public Vector2Int size;
        public PlacementMethodType placementType;
        public RotationStrategyType rotationType;
        public Color initialColor = Color.white;
        private IPlacementMethod _placementMethod;

        public Sprite Icon => icon;
        public GameObject Prefab => prefab;
        public Vector2Int Size => size;
        public Color InitialColor => initialColor;
        

        public PlacementMethodType PlacementMethodType => placementType;

        public IPlacementMethod PlacementMethod
        {
            get
            {
                if (_placementMethod == null)
                    switch (placementType)
                    {
                        case PlacementMethodType.Placement:
                            _placementMethod = new FloorPlacementMethod();
                            break;
                        case PlacementMethodType.WallPlacement:
                            _placementMethod = new WallPlacementMethod();
                            break;
                        case PlacementMethodType.DanceFloor:
                            _placementMethod = new DanceFloorPlacementMethod();
                            break;
                        default:
                            _placementMethod = new NonePlacementMethod();
                            break;
                    }

                return _placementMethod;
            }
        }

        public IRotationMethod RotationMethod => rotationType switch
        {
            RotationStrategyType.ThreeSixty => new RotationMethodler360(),
            RotationStrategyType.LeftAndDown => new RotationMethodLeftAndDown(),
            _ => new NoneRotationMethod()
        };

        public IColorChanger ColorChanger => new SimpleColorChanger();
    }

    public enum PlacementMethodType
    {
        None,
        Placement,
        WallPlacement,
        DanceFloor,
    }

   

    public enum RotationStrategyType
    {
        None,
        ThreeSixty,
        LeftAndDown,
        Auto
    }

    public interface IPlaceableItemData
    {
        Sprite Icon { get; }
        GameObject Prefab { get; }
        Vector2Int Size { get; }
        public Color InitialColor { get; }
        PlacementMethodType PlacementMethodType { get; }
        IPlacementMethod PlacementMethod { get; }
        IRotationMethod RotationMethod { get; }
        IColorChanger ColorChanger { get; }
    }

    public interface IPlacementMethod
    {
        bool pressAndHold { get; }
        Vector3 offset { get; }
        bool CanPlace(Vector3Int cellPos, IPlaceableItemData placeableItemData, RotationData rotationData);
        void LogicUpdate(Vector3Int cellPos, IPlaceableItemData placeableItemData, RotationData rotationData);

        GameObject Place(Vector3Int cellPosInt, Vector3 cellPos, IPlaceableItemData placeableItemData,
            RotationData rotationData);
    }

    public interface IColorChanger
    {
        void ChangeColor(GameObject item, Color color);
    }

    public interface IRotationMethod
    {
        RotationData GetRotation(RotationData rotationData);
    }

    public interface IBuilder
    {
        void Setup(IPlaceableItemData placeableItemData);
        void BuildUpdate();
        void Exit();
    }

    public class SimpleColorChanger : IColorChanger
    {
        public void ChangeColor(GameObject item, Color color)
        {
            var renderers = item.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers) renderer.material.color = color;
        }
    }
}