using System.Collections.Generic;
using System.Linq;
using Data;
using DiscoSystem.Character.Bartender.Command;
using Framework.Context;
using Framework.Mvcs.View;
using Prop_Behaviours.Bar;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;

namespace UI.GamePages
{
    public class UIPickADrinkPage : BaseView
    {
        private UI_FollowTarget _followTarget;
        private int _lastInstanceID = -1;
        private Bar _lastBar;

        [SerializeField] private GameObject _uiDrinkSlotPrefab;
        [SerializeField] private Transform _drinkContent;

        public override PageType PageType { get; protected set; } = PageType.MiniPage;
        
        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            
            _followTarget = GetComponent<UI_FollowTarget>();
            LoadDrinks(GameBundle.Instance.AllInGameDrinks.Values.ToList());
        }

        public void Show(Bar bar)
        {
            if (_lastInstanceID == bar.GetInstanceID() && IsToggled)
            {
                ToggleView(false);
                return;
            }
            
            _lastInstanceID = bar.GetInstanceID();
            _lastBar = bar;
            ToggleView(true);

            
            _followTarget.SetTarget(bar.gameObject);
            UpdateVisual();
        }
        
 
        private void UpdateVisual()
        {
            // Update Visuals
        }

        public void SelectADrink(DrinkSO selectedDrinkSo)
        {
            BarMediator.Instance.AddCommand(_lastBar, new PrepareDrinkCommand(selectedDrinkSo));
            ToggleView(false);
        }

        private void LoadDrinks(List<DrinkSO> drinkItems)
        {
            for (int i = _drinkContent.childCount - 1; i >= 0; i--)
                Destroy(_drinkContent.GetChild(i).gameObject);
            
            foreach (var drink in drinkItems)
            {
                var obj = Instantiate(_uiDrinkSlotPrefab, _drinkContent);
                var drinkSlot = obj.GetComponent<UIDrinkSlot>();
                drinkSlot.Init(drink, this);
            }
        }
    }
}