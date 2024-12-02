using UnityEngine;

namespace Disco_ScriptableObject
{
    [CreateAssetMenu(fileName = "New Extend Item", menuName = "Item/Extend Item")]
    public class ExtendItemSo : StoreItemSO
    {
        public int ExtendX;
        public int ExtendY;
    }
}