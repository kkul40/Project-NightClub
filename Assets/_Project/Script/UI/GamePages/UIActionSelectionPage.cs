using DG.Tweening;
using PropBehaviours;
using UnityEngine;

namespace UI.GamePages
{
    public class UIActionSelectionPage : UIPageBase
    {
        public override PageType PageType { get; protected set; } = PageType.MiniPage;

        private UI_FollowTarget _followTarget;

        [SerializeField] private GameObject InfoButton;
        [SerializeField] private GameObject DrinkButton;

        private RectTransform InfoButtonRect;
        
        private IPropUnit lastPropUnit;

        protected override void OnAwake()
        {
            CloseAll();
            _followTarget = GetComponent<UI_FollowTarget>();
        }

        protected override void OnShow<T>(T data)
        {
            IPropUnit propUnit = data as IPropUnit;

            lastPropUnit = propUnit;
            _followTarget.SetTarget(lastPropUnit.gameObject);
            
            CloseAll();

            if (propUnit is Bar)
            {
                InfoButton.SetActive(true);
                DrinkButton.SetActive(true);
            }
            else
            {
                InfoButton.SetActive(true);
            }
        }

        private void CloseAll()
        {
            InfoButton.SetActive(false);
            DrinkButton.SetActive(false);
        }

        public void OpenInfoPage()
        {
            UIPageManager.Instance.RequestAPage(typeof(UIPropInfo), lastPropUnit);
        }

        public void OpenDrinkPage()
        {
            UIPageManager.Instance.RequestAPage(typeof(UIBarPage), lastPropUnit);
        }
    }
}