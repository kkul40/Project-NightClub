using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Disco_Building;
using PropBehaviours;
using UnityEngine;

namespace UI.GamePages
{
    public class UIActionSelectionPage : UIPageBase
    {
        public override PageType PageType { get; protected set; } = PageType.MiniPage;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private List<GameObject> _allButtons = new List<GameObject>();

        [Header("Buttons")]
        [SerializeField] private GameObject InfoButton;
        [SerializeField] private GameObject DrinkButton;
        [SerializeField] private GameObject CancelButton;
        [SerializeField] private GameObject RelocateButton;
        [SerializeField] private GameObject RelocateDoorButon;
        [SerializeField] private GameObject RemoveButton;

        [Header("Circle Placement Settings")]
        public float radius = 100f;
        public float angleBetween = 30f;

        private UI_FollowTarget _followTarget;
        private object _lastData;
        private readonly Dictionary<Type, List<Action<object>>> _typeBehaviors = new();
        private List<Tween> _tweens = new();

        protected override void OnAwake()
        {
            CloseAllButtons();
            _followTarget = GetComponent<UI_FollowTarget>();

            RegisterBehaviors();
        }

        protected override void OnShow<T>(T data)
        {
            _lastData = data;

            CloseAllButtons();
            Invoke(nameof(ActivateButtonsAndArrange), 0.1f);
            // ActivateButtonsAndArrange();
        }

        private void ActivateButtonsAndArrange()
        {
            ActivateButtonsBasedOnData();
            ArrangeButtonsInCircle();
        }

        private void RegisterBehaviors()
        {
            AddBehavior<IPropUnit>(data =>
            {
                EnableButtons(InfoButton, RelocateButton, RemoveButton);
            });

            AddBehavior<Bar>(data =>
            {
                EnableButtons(DrinkButton);
            });

            AddBehavior<Bartender>(data =>
            {
                EnableButtons(CancelButton);
            });

            AddBehavior<WallDoor>(data =>
            {
                EnableButtons(RelocateDoorButon);
            });
        }

        private void ActivateButtonsBasedOnData()
        {
            if (_lastData == null) return;

            Type currentType = _lastData.GetType();

            // Traverse type hierarchy and apply all registered behaviors
            while (currentType != null)
            {
                if (_typeBehaviors.TryGetValue(currentType, out var actions))
                {
                    foreach (var action in actions)
                        action(_lastData);
                }

                currentType = currentType.BaseType;
            }
            
            SetFollowTarget(_lastData);
        }

        private void AddBehavior<T>(Action<object> action)
        {
            var type = typeof(T);
            if (!_typeBehaviors.ContainsKey(type))
            {
                _typeBehaviors[type] = new List<Action<object>>();
            }

            _typeBehaviors[type].Add(action);
        }

        private void SetFollowTarget(object data)
        {
            var property = data.GetType().GetProperty("gameObject");
            if (property != null)
            {
                var targetGameObject = property.GetValue(data) as GameObject;
                _followTarget.SetTarget(targetGameObject);
            }
        }
  
        private void EnableButtons(params GameObject[] buttons)
        {
            foreach (var button in buttons)
            {
                button.SetActive(true);
            }
        }

        private void ArrangeButtonsInCircle()
        {
            float moveDuration = 0.5f;
            float fadeDuration = 0.5f;
            
            var activeButtons = _allButtons.Where(button => button.activeInHierarchy).ToList();
            if (activeButtons.Count == 0) return;

            float fixedRadius = Screen.height * radius / _canvas.scaleFactor;
            float totalArcAngle = (activeButtons.Count - 1) * angleBetween;
            float startAngle = Mathf.PI / 2 - (totalArcAngle / 2 * Mathf.Deg2Rad);

            // Clear exist tweens
            _tweens.ForEach(t => t.Kill());
            _tweens.Clear();

            // Position buttons in a circular arrangement
            for (int i = 0; i < activeButtons.Count; i++)
            {
                var rectTransform = activeButtons[i].GetComponent<RectTransform>();
                var canvasGroup = activeButtons[i].GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                
                float angle = startAngle + i * angleBetween * Mathf.Deg2Rad;
                Vector2 targetPoint = new Vector2(Mathf.Cos(angle) * fixedRadius, Mathf.Sin(angle) * fixedRadius);

                rectTransform.anchoredPosition = Vector3.zero;
                _tweens.Add(rectTransform.DOAnchorPos(targetPoint, moveDuration).SetEase(Ease.OutExpo).SetLink(rectTransform.gameObject));
                _tweens.Add(canvasGroup.DOFade(1, fadeDuration));
            }
        }

        private void CloseAllButtons()
        {
            foreach (var button in _allButtons)
                button.SetActive(false);
        }

        public void OpenPropInfo()
        {
            if (_lastData is IPropUnit propUnit)
            {
                UIPageManager.Instance.RequestAPage(typeof(UIPropInfo), propUnit);
                Hide();
            }
        }

        public void OpenDrinkPage()
        {
            if (_lastData is Bar bar)
            {
                UIPageManager.Instance.RequestAPage(typeof(UIPickADrinkPage), bar);
                Hide();
            }
        }

        public void Relocate()
        {
            if (_lastData is IPropUnit propUnit)
            {
                var item = DiscoData.Instance.FindAItemByID(propUnit.ID);
                BuildingManager.Instance.ReplaceObject(item, propUnit.CellPosition, propUnit.PlacementLayer);
                Hide();
            }
        }

        public void RelocateDoor()
        {
            if (_lastData is WallDoor wallDoor)
            {
                BuildingManager.Instance.ChangeDoorPosition(wallDoor);
                Hide();
            }
        }

        public void Remove()
        {
            if (_lastData is IPropUnit propUnit)
            {
                DiscoData.Instance.placementDataHandler.RemovePlacement(propUnit.CellPosition, propUnit.PlacementLayer, true);
                Hide();
            }
        }
    }
}
