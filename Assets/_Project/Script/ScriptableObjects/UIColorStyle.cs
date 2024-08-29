using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UI Color Style", menuName = "New UI Color Style")]
    public class UIColorStyle : ScriptableObject
    {
        public Color UIWindowColor;
        public Color UIInnerWindowColor;
        public Color UIButtonCollor;
    }
}