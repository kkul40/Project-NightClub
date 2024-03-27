using TMPro;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Text Style")]
    public class TextStyleSo : ScriptableObject
    {
        public TMP_FontAsset FontAsset;
        public float FontSize;
    }
}