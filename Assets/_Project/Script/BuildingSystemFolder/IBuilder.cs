using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public interface IBuilder
    {
        public void Setup(PlacablePropSo placablePropSo);
        public void BuildUpdate();
        public void Exit();
    }
}