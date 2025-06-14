using _Initializer;
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
        }

        private void Start()
        {
            _textMeshPro.font = ServiceLocator.Get<InitConfig>().GetDefaultUITextStyle.UITextAsset;
        }
    }
}