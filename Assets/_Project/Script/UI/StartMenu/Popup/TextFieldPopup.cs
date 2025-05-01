using System;
using Framework.Mvcs.View;
using TMPro;
using UnityEngine;

namespace UI.StartMenu.Popup
{
    public class TextFieldPopup : BaseView
    {
        [SerializeField] private GameObject _bg;
        public override PageType PageType { get; protected set; } = PageType.PopUp;
        private Action<string> _callBack;

        [SerializeField] private TextMeshProUGUI _messageBox;

        [SerializeField] private TMP_InputField _inputField;

        public void Show(string message, Action<string> callBack)
        {
            ToggleView(true);
            _messageBox.text = message;
            _callBack = callBack;
        }

        public void OnDone()
        {
            _callBack?.Invoke(_inputField.text);
            ToggleView(false);
        }

        public void OnClose()
        {
            _callBack = null;
            ToggleView(false);
        }

        public override void ToggleView(bool toggle)
        {
            base.ToggleView(toggle);
            _bg.SetActive(toggle);
        }
    }
}