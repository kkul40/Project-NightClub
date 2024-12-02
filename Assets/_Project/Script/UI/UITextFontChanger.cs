using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UITextFontChanger : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
            _textMeshPro.font = InitConfig.Instance.GetDefaultUITextStyle.UITextAsset;
        }
    }
}