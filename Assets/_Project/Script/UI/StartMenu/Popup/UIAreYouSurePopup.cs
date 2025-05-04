using System;
using DiscoSystem.Building_System.GameEvents;
using Framework.Mvcs.View;
using TMPro;
using UnityEngine;

namespace UI.StartMenu.Popup
{
    public class UIAreYouSurePopup : BaseView
    {
        [SerializeField] private GameObject _bg;

        public override PageType PageType { get; protected set; } = PageType.PopUp;

        private Action _callBack;
        [SerializeField] private TextMeshProUGUI _message;

        public void Show(string message, Action callBack)
        {
            _message.text = message;
            _callBack = callBack;
            ToggleView(true);
        }

        public void Yes()
        {
            _callBack?.Invoke();
            ToggleView(false);
        }

        public void Cancel()
        {
            _callBack = null;
            ToggleView(false);
        }

        public override void ToggleView(bool toggle)
        {
            base.ToggleView(toggle);
            GameEvent.Trigger(new Event_ToggleInputs(!toggle));
            _bg.SetActive(toggle);
        }
    }
}