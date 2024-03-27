using ScriptableObjects;

namespace BuildingSystemFolder
{
    public interface IBuild
    {
        public void Setup<T>(T itemSo) where T : ItemSo;
        public void BuildUpdate();
        public void Exit();
    }
}