using Data;
using Disco_ScriptableObject;
using Framework.Context;
using Framework.Mvcs.View;
using PropBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GamePages
{
    public class UIPropInfo : BaseView
    {
        private int _lastInstanceID = -1;
        private IPropUnit _lastINT;

        private UI_FollowTarget _followTarget;

        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _textDescription;

        public override PageType PageType { get; protected set; } = PageType.MiniPage;

        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            _followTarget = GetComponent<UI_FollowTarget>();

        }

        public void Show(IPropUnit unit)
        {
            if (_lastInstanceID == unit.GetInstanceID() && IsToggled)
            {
                ToggleView(false);
                return;
            }
            
            _lastInstanceID = unit.GetInstanceID();
            _lastINT = unit;
            ToggleView(true);
            
            _followTarget.SetTarget(unit.gameObject);
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            StoreItemSO item = GameBundle.Instance.FindAItemByID(_lastINT.ID);

            _image.sprite= item.Icon;
            _text.text = item.Name;
            _textDescription.text = item.Description;
        }
    }
}