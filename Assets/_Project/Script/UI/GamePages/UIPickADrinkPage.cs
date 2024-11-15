using Data;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;

namespace UI.GamePages
{
    public class UIPickADrinkPage : UIPageBase
    {
        private UI_FollowTarget _followTarget;
        private int _lastInstanceID = -1;
        private bool _toggle;
        private Bar _lastBar;

        [SerializeField] private GameObject _uiDrinkSlotPrefab;
        [SerializeField] private Transform _drinkContent;

        public override PageType PageType { get; protected set; } = PageType.MiniPage;
        
        protected override void OnAwake()
        {
            _followTarget = GetComponent<UI_FollowTarget>();
        }

        private void Start()
        {
            LoadDrinks();
        }

        protected override void OnShow<T>(T data)
        {
            Bar bar = data as Bar;
            
            if (_lastInstanceID == bar.GetInstanceID() && _toggle)
            {
                Hide();
                return;
            }
            
            _lastInstanceID = bar.GetInstanceID();
            _lastBar = bar;
            _toggle = true;
            
            _followTarget.SetTarget(bar.gameObject);
            UpdateVisual();
        }

        protected override void OnHide()
        {
            _toggle = false;
        }

        private void UpdateVisual()
        {
            // Update Visuals
        }

        public void SelectADrink(DrinkSO selectedDrinkSo)
        {
            BarMediator.Instance.AddCommand(_lastBar, new PrepareDrinkCommand(selectedDrinkSo));
            Hide();
        }

        private void LoadDrinks()
        {
            foreach (var drink in DiscoData.Instance.AllInGameDrinks)
            {
                var obj = Instantiate(_uiDrinkSlotPrefab, _drinkContent);
                var drinkSlot = obj.GetComponent<UIDrinkSlot>();
                drinkSlot.Init(drink.Value, this);
            }
        }
    }
}