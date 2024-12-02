using TMPro;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UI Text Style", menuName = "UI/Text Color Style")]
    public class UITextStyle : ScriptableObject
    {
        public TMP_FontAsset UITextAsset;
    }
}