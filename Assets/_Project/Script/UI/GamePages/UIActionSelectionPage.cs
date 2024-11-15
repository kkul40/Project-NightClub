using System;
using System.Collections.Generic;
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
        [SerializeField] private GameObject RelocateButton;
        [SerializeField] private GameObject RemoveButton;

        private RectTransform InfoButtonRect;
        private int _lastInstanceID;
        private IPropUnit _lastPropUnit;
        
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
            _lastPropUnit = data as IPropUnit;

            _lastInstanceID = data.GetHashCode();
            
            _followTarget.SetTarget(_lastPropUnit.gameObject);
            CloseAllButtons();
            Invoke(nameof(SetUpButtons), 0.1f);
        }
        
        private void SetUpButtons()
        {
            // Activate Buttons
            if (_lastPropUnit is IPropUnit)
            {
                InfoButton.SetActive(true);
                RelocateButton.SetActive(true);
                RemoveButton.SetActive(true);
            }
            
            if (_lastPropUnit is Bar)
            {
                DrinkButton.SetActive(true);
            }
            
            // Position Buttons
            List<GameObject> activeButtons = new List<GameObject>();
            for (int i = 0; i < _allButtons.Count; i++)
            {
                if (_allButtons[i].activeInHierarchy)
                    activeButtons.Add(_allButtons[i]);
            }

            if (activeButtons.Count == 0) return;

            List<Vector2> endPoints = new List<Vector2>();
            GenerateCirclePoints(activeButtons.Count, radius, angleBetween, out endPoints);


            foreach (var tween in _tweens)
            {
                tween.Kill();
            }
            
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
            UIPageManager.Instance.RequestAPage(typeof(UIPropInfo), _lastPropUnit);
        }

        public void OpenDrinkPage()
        {
            UIPageManager.Instance.RequestAPage(typeof(UIPickADrinkPage), _lastPropUnit);
        }
        
        public void Relocate()
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
        
        private void GenerateCirclePoints(int numberOfPoints, float radius, float angleBetweenPoints, out List<Vector2> pointPositions)
        {
            float fixedRadius = Screen.height * radius / _canvas.scaleFactor;
            Debug.Log(Screen.height);
            pointPositions = new List<Vector2>();
            float totalArcAngle = (numberOfPoints - 1) * angleBetweenPoints;
        
            float startAngle = Mathf.PI / 2 - (totalArcAngle / 2 * Mathf.Deg2Rad); 

            for (int i = 0; i < numberOfPoints; i++)
            {
                float angle = startAngle + i * angleBetweenPoints * Mathf.Deg2Rad;

                float x = Mathf.Cos(angle) * fixedRadius;
                float y = Mathf.Sin(angle) * fixedRadius;

                pointPositions.Add(new Vector2(x, y));
            }
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