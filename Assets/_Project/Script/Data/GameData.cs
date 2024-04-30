using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameData : Singleton<GameData>
{
    [SerializeField] private PlacamentDataHandler _placamentDataHandler;
    [SerializeField] private MapGeneratorSystem _mapGeneratorSystem;

    [Header("Data")] 
    public List<Vector3> FloorMap = new ();
    public List<Wall> WallMap = new();

    private void Awake()
    {
        _placamentDataHandler = new PlacamentDataHandler();
    }
    
    public List<Wall> GetWallMapPosList => WallMap;
    public List<Prop> GetPropList => PlacementHandler.GetPropList;
    public PlacamentDataHandler PlacementHandler => _placamentDataHandler;
    public MapGeneratorSystem MapGenerator => _mapGeneratorSystem;
}