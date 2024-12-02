using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UI Color Style", menuName = "UI/UI Color Style")]
    public class UIColorStyle : ScriptableObject
    {
        public Color UIWindowColor;
        public Color UIInnerWindowColor;
        public Color UIButtonCollor;
    }
}