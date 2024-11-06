using UnityEngine;

namespace Disco_ScriptableObject
{
    [CreateAssetMenu(menuName = "New Item/New Extend Item")]
    public class ExtendItemSo : StoreItemSO
    {
        public int ExtendX;
        public int ExtendY;
    }
}