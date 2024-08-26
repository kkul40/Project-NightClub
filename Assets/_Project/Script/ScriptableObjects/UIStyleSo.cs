using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New UI Style")]
    public class UIStyleSo : ScriptableObject
    {
        public Color bir;
        public Color iki;
        public Color uc;
        public Color dort;
        public Color bes;
        
        public Color WindowColor;
        public Color InnerWindowColor;
        public Color ButtonColor;
    }
}