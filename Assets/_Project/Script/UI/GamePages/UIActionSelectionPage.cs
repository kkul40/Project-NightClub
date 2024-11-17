using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Disco_Building;
using Disco_ScriptableObject;
using PropBehaviours;
using UnityEngine;

namespace UI.GamePages
{
    public class UIActionSelectionPage : UIPageBase
    {
        public override PageType PageType { get; protected set; } = PageType.MiniPage;

        [SerializeField] private Canvas _canvas;
        private RectTransform _rectTransform;
        private UI_FollowTarget _followTarget;
        
        [Header("Order Of Buttons")]
        [SerializeField] private List<GameObject> _allButtons = new List<GameObject>();

        [Header("Buttons")]
        [SerializeField] private GameObject InfoButton;
        [SerializeField] private GameObject DrinkButton;
        [SerializeField] private GameObject CancelButton;
        [SerializeField] private GameObject RelocateButton;
        [SerializeField] private GameObject RemoveButton;

        private RectTransform InfoButtonRect;
        private int _lastInstanceID;
        private object _lastData;
        
        [Header("Circle Placement Settings")]
        public int pointCoutn;
        public float radius;
        public float angleBetween;

        private List<Tween> _tweens = new List<Tween>();

        protected override void OnAwake()
        {
            CloseAllButtons();
            _followTarget = GetComponent<UI_FollowTarget>();
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnShow<T>(T data)
        {
            _lastData = data;
            
            CloseAllButtons();
            Invoke(nameof(SetUpButtons), 0.1f);
        }

        private void SetUpButtons()
        {
            HandleDataAndActivateButton();
            ArrangeButtonInCircle();
        }

        private void HandleDataAndActivateButton()
        {
            if (_lastData is IPropUnit lastProp)
            {
                InfoButton.SetActive(true);
                RelocateButton.SetActive(true);
                RemoveButton.SetActive(true);
                _followTarget.SetTarget(lastProp.gameObject);
            }

            if (_lastData is Bar _lastBar)
            {
                DrinkButton.SetActive(true);
                _followTarget.SetTarget(_lastBar.gameObject);
            }

            if (_lastData is Bartender _lastBartender)
            {
                CancelButton.SetActive(true);
                _followTarget.SetTarget(_lastBartender.gameObject);
            }
        }
        
        private void ArrangeButtonInCircle()
        {
            var activeButtons = _allButtons.Where(button => button.activeInHierarchy).ToList();
            if (activeButtons.Count == 0) return;

            List<Vector2> endPoints = GenerateCirclePoints(activeButtons.Count, radius, angleBetween);

            _tweens.ForEach(t => t.Kill());
            _tweens.Clear();
            
            for (int i = 0; i < activeButtons.Count; i++)
            {
                RectTransform rectTransform = activeButtons[i].GetComponent<RectTransform>();
                Vector2 targetPoint = new Vector2(endPoints[i].x, endPoints[i].y);
                rectTransform.anchoredPosition = Vector3.zero;
                _tweens.Add(rectTransform.DOAnchorPos(targetPoint, 0.5f).SetEase(Ease.OutExpo).SetLink(rectTransform.gameObject));
            }
        }

        private void CloseAllButtons()
        {
            foreach (var button in _allButtons)
                button.SetActive(false);
        }

        public void OpenPropInfo()
        {
            var propUnit = _lastData as IPropUnit;
            if (propUnit == null) return;
            UIPageManager.Instance.RequestAPage(typeof(UIPropInfo), _lastData as IPropUnit);
            Hide();
        }

        public void OpenDrinkPage()
        {
            var bar = _lastData as Bar;
            if (bar == null) return;
            UIPageManager.Instance.RequestAPage(typeof(UIPickADrinkPage), bar);
            Hide();
        }

        public void HandleCancelButton()
        {
            
        }
        
        public void Relocate()
        {
            var propUnit = _lastData as IPropUnit;
            if (propUnit == null) return;
            StoreItemSO item = DiscoData.Instance.FindAItemByID(propUnit.ID);
            BuildingManager.Instance.ReplaceObject(item, propUnit.CellPosition, propUnit.PlacementLayer);
            Hide();
        }

        public void Remove()
        {
            var propUnit = _lastData as IPropUnit;
            if (propUnit == null) return;
            DiscoData.Instance.placementDataHandler.RemovePlacement(propUnit.CellPosition, propUnit.PlacementLayer, true);
            Hide();
        }
        
        private List<Vector2> GenerateCirclePoints(int numberOfPoints, float radius, float angleBetweenPoints)
        {
            float fixedRadius = Screen.height * radius / _canvas.scaleFactor;
            List<Vector2> pointPositions = new List<Vector2>();
            
            float totalArcAngle = (numberOfPoints - 1) * angleBetweenPoints;
            float startAngle = Mathf.PI / 2 - (totalArcAngle / 2 * Mathf.Deg2Rad); 

            for (int i = 0; i < numberOfPoints; i++)
            {
                float angle = startAngle + i * angleBetweenPoints * Mathf.Deg2Rad;

                float x = Mathf.Cos(angle) * fixedRadius;
                float y = Mathf.Sin(angle) * fixedRadius;

                pointPositions.Add(new Vector2(x, y));
            }

            return pointPositions;
        }
       
        private void OnDrawGizmos()
        {
            // if (_rectTransform == null) return;
            // if (angleBetween <= 0) return;
            // List<Vector2> endPoints = new List<Vector2>();
            // GenerateCirclePoints(pointCoutn, radius, angleBetween, out endPoints);
            //
            // foreach (var point in endPoints)
            // {
            //     Gizmos.color = Color.red;
            //     Gizmos.DrawSphere(_rectTransform.position + new Vector3(point.x, point.y, 0), 5);
            // }
        }
    }
}