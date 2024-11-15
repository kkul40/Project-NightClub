using System;
using System.Collections.Generic;
using PropBehaviours;
using UnityEngine;

namespace UI.GamePages
{
    public class UIActionSelectionPage : UIPageBase
    {
        public override PageType PageType { get; protected set; } = PageType.MiniPage;

        private RectTransform _rectTransform;
        private UI_FollowTarget _followTarget;
        private List<GameObject> _allButtons = new List<GameObject>();

        [SerializeField] private GameObject InfoButton;
        [SerializeField] private GameObject RelocateButton;
        [SerializeField] private GameObject DrinkButton;

        private RectTransform InfoButtonRect;
        
        private IPropUnit _lastPropUnit;
        
        public int pointCoutn;
        public float radius;
        public float angleBetween;

        protected override void OnAwake()
        {
            CloseAll();
            _followTarget = GetComponent<UI_FollowTarget>();
            _rectTransform = GetComponent<RectTransform>();
            
            _allButtons.Add(DrinkButton);
            _allButtons.Add(RelocateButton);
            _allButtons.Add(InfoButton);
        }

        protected override void OnShow<T>(T data)
        {
            IPropUnit propUnit = data as IPropUnit;

            _lastPropUnit = propUnit;
            _followTarget.SetTarget(_lastPropUnit.gameObject);
            
            CloseAll();

            if (propUnit is Bar)
            {
                InfoButton.SetActive(true);
                RelocateButton.SetActive(true);
                DrinkButton.SetActive(true);
            }
            else
            {
                InfoButton.SetActive(true);
                RelocateButton.SetActive(true);
            }
            
            SetButtonPosition();
        }
        
        private void Update()
        {
            // SetButtonPosition();
        }

        private void SetButtonPosition()
        {
            List<GameObject> activeButtons = new List<GameObject>();
            for (int i = 0; i < _allButtons.Count; i++)
            {
                if (_allButtons[i].activeInHierarchy)
                    activeButtons.Add(_allButtons[i]);
            }

            if (activeButtons.Count == 0) return;

            Debug.Log(activeButtons.Count);
            List<Vector2> endPoints = new List<Vector2>();
            GenerateCirclePoints(activeButtons.Count, radius, angleBetween, out endPoints);

            for (int i = 0; i < activeButtons.Count; i++)
            {
                RectTransform rectTransform = activeButtons[i].GetComponent<RectTransform>();
                Vector3 endPoint = new Vector3(endPoints[i].x, endPoints[i].y, 0);
                rectTransform.position = _rectTransform.position + endPoint;
            }
        }
        
        private void CloseAll()
        {
            foreach (var button in _allButtons)
                button.SetActive(false);
        }

        public void OpenInfoPage()
        {
        }

        public void RelocatePage()
        {
            UIPageManager.Instance.RequestAPage(typeof(UIPropRelocatePage), _lastPropUnit);
        }

        public void OpenDrinkPage()
        {
            UIPageManager.Instance.RequestAPage(typeof(UIPickADrinkPage), _lastPropUnit);
        }
        
        private void GenerateCirclePoints(int numberOfPoints, float radius, float angleBetweenPoints, out List<Vector2> pointPositions)
        {
            pointPositions = new List<Vector2>();
            float totalArcAngle = (numberOfPoints - 1) * angleBetweenPoints;
        
            float startAngle = Mathf.PI / 2 - (totalArcAngle / 2 * Mathf.Deg2Rad); 

            for (int i = 0; i < numberOfPoints; i++)
            {
                float angle = startAngle + i * angleBetweenPoints * Mathf.Deg2Rad;

                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                pointPositions.Add(new Vector2(x, y));
            }
        }
       
        private void OnDrawGizmos()
        {
            if (_rectTransform == null) return;
            if (angleBetween <= 0) return;
            List<Vector2> endPoints = new List<Vector2>();
            GenerateCirclePoints(pointCoutn, radius, angleBetween, out endPoints);

            foreach (var point in endPoints)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_rectTransform.position + new Vector3(point.x, point.y, 0), 5);
            }
        }
    }
}