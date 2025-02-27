using Data;
using Disco_ScriptableObject;
using PropBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GamePages
{
    public class UIPropInfo : UIPageBase
    {
        private bool _toggle;
        private int _lastInstanceID = -1;
        private IPropUnit _lastINT;

        private UI_FollowTarget _followTarget;

        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _textDescription;

        public override PageType PageType { get; protected set; } = PageType.MiniPage;

        protected override void OnAwake()
        {
            _followTarget = GetComponent<UI_FollowTarget>();
        }

        protected override void OnShow<T>(T data)
        {
            var propUnit = data as IPropUnit;

            if (_lastInstanceID == propUnit.GetInstanceID() && _toggle)
            {
                Hide();
                _toggle = false;
                return;
            }
            
            _lastInstanceID = propUnit.GetInstanceID();
            _lastINT = propUnit;
            _toggle = true;
            
            _followTarget.SetTarget(propUnit.gameObject);
            UpdateVisual();
        }

        protected override void OnHide()
        {
            _toggle = false;
        }

        private void UpdateVisual()
        {
            StoreItemSO item = DiscoData.Instance.FindAItemByID(_lastINT.ID);

            _image.sprite= item.Icon;
            _text.text = item.Name;
            _textDescription.text = item.Description;
        }
    }
}