using Data;
using Disco_Building;
using Disco_Building.Builders;
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
        private IPropUnit _lastPropUnit;

        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        
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
            _lastPropUnit = propUnit;
            _toggle = true;
            
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            StoreItemSO item = DiscoData.Instance.FindAItemByID(_lastPropUnit.ID);

            _image.sprite= item.Icon;
            _text.text = item.Name;
        }

        public void Replace()
        {
            StoreItemSO item = DiscoData.Instance.FindAItemByID(_lastPropUnit.ID);
            BuildingManager.Instance.ReplaceObject(item, _lastPropUnit.CellPosition, _lastPropUnit.PlacementLayer);
            Hide();
        }

        public void Remove()
        {
            DiscoData.Instance.placementDataHandler.RemovePlacement(_lastPropUnit.CellPosition, _lastPropUnit.PlacementLayer, true);
            Hide();
        }
    }
}