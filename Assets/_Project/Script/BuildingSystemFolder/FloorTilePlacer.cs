using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class FloorTilePlacer : MonoBehaviour, IBuild
    {
        public void Setup<T>(T itemSo) where T : ItemSo
        {
            throw new System.NotImplementedException();
        }

        public void BuildUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}