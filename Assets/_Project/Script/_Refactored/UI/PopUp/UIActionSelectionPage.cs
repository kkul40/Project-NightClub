using System;
using System.Collections.Generic;
using System.Linq;
using _Initializer;
using DG.Tweening;
using DiscoSystem;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Character.Bartender;
using DiscoSystem.MusicPlayer;
using Framework.Context;
using Framework.Mvcs.View;
using PropBehaviours;
using UI.GamePages;
using UI.GamePages.GameButtons;
using UnityEngine;

namespace UI.PopUp
{
    public class UIActionSelectionPage : BaseView
    {
        public override PageType PageType { get; protected set; } = PageType.MiniPage;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private List<GameObject> _allButtons = new List<GameObject>();

        [Header("Buttons")]
        [SerializeField] private GameObject InfoButton;
        [SerializeField] private GameObject DrinkButton;
        [SerializeField] private GameObject MusicButton;
        [SerializeField] private GameObject CancelButton;
        [SerializeField] private GameObject RelocateButton;
        [SerializeField] private GameObject RelocateDoorButon;
        [SerializeField] private GameObject RemoveButton;

        [Header("Circle Placement Settings")]
        public float radius = 100f;
        public float angleBetween = 30f;
        public float startAngle = 90f;

        private UI_FollowTarget _followTarget;
        private object _lastData;
        private readonly Dictionary<Type, List<Action<object>>> _typeBehaviors = new();
        private List<Tween> _tweens = new();

        private void Awake()
        {
            CloseAllButtons();
        }

        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            _followTarget = GetComponent<UI_FollowTarget>();

            GameEvent.Subscribe<Event_StartedPlacing>(handle =>
            {
                ToggleView(false);
                _lastData = null;
            });
            
            RegisterBehaviors();
            
            GameEvent.Subscribe<Event_SelectionReset>(handle =>
            {
                ToggleView(false);
                _lastData = null;
            });
        }

        private void Update()
        {
            if (_lastData == null) return;
        }

        public override void EventEnable()
        {
        }

        public override void EventDisable()
        {
            CancelInvoke();
        }

        public void Show(object data)
        {
            _lastData = data;

            CloseAllButtons();
            ToggleView(true);
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
                SetFollowTarget(data);
            });

            AddBehavior<Bar>(data =>
            {
                EnableButtons(DrinkButton);
                SetFollowTarget(data);
            });

            AddBehavior<Bartender>(data =>
            {
                EnableButtons(CancelButton);
                SetFollowTarget(data);
            });

            AddBehavior<WallDoor>(data =>
            {
                EnableButtons(RelocateDoorButon);
                SetFollowTarget(data);
            });

            AddBehavior<DJ>(data =>
            {
                EnableButtons(MusicButton);
                SetFollowTarget(data);
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
            // float startAngle = Mathf.PI / 2 - (totalArcAngle / 2 * Mathf.Deg2Rad);
            float sAngle = startAngle * Mathf.Deg2Rad; // Set to exactly 90 degrees
            
            // Clear exist tweens
            _tweens.ForEach(t => t.Kill());
            _tweens.Clear();

            // Position buttons in a circular arrangement
            for (int i = 0; i < activeButtons.Count; i++)
            {
                var rectTransform = activeButtons[i].GetComponent<RectTransform>();
                var canvasGroup = activeButtons[i].GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                
                float angle = sAngle + i * angleBetween * Mathf.Deg2Rad;
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
                ServiceLocator.Get<UIPageManager>().ShowPropInfo(propUnit);
                PlaySFXOnButtonClick();
                ToggleView(false);
            }
        }

        public void OpenDrinkPage()
        {
            if (_lastData is Bar bar)
            {
                ServiceLocator.Get<UIPageManager>().ShowDrinkPage(bar);
                PlaySFXOnButtonClick();
                ToggleView(false);
            }
        }

        public void OpenSongPage()
        {
            if (_lastData is DJ dj)
            {
                ServiceLocator.Get<DJMusicManager>().PlayeNextSong();
                PlaySFXOnButtonClick();
            }
        }

        public void Relocate()
        {
            if (_lastData is IPropUnit propUnit)
            {
                var instanceID = propUnit.transform.GetInstanceID();
                GameEvent.Trigger(new Event_RelocatePlacement(instanceID));
                PlaySFXOnButtonClick();
                ToggleView(false);
                GameEvent.Trigger(new Event_ResetCursorSelection());
            }
        }

        public void RelocateDoor()
        {
            if (_lastData is WallDoor wallDoor)
            {
                GameEvent.Trigger(new Event_RelocateWallDoor(wallDoor));
                PlaySFXOnButtonClick();
                ToggleView(false);

                GameEvent.Trigger(new Event_ResetCursorSelection());
            }
        }

        public void Cancel()
        {
            if (_lastData is Bartender bartender)
            {
                Debug.Log("Bartender's current Task Will Cancelled");
                // TODO : Add The logic Here
                PlaySFXOnButtonClick();
                ToggleView(false);
                GameEvent.Trigger(new Event_ResetCursorSelection());
            }
        }

        public void Remove()
        {
            if (_lastData is IPropUnit propUnit)
            {
                GameEvent.Trigger(new Event_RemovePlacement(propUnit.transform.GetInstanceID()));
                PlaySFXOnButtonClick();
                ToggleView(false);
                GameEvent.Trigger(new Event_ResetCursorSelection());
            }
        }

        private void PlaySFXOnButtonClick()
        {
            GameEvent.Trigger(new Event_Sfx(SoundFXType.Click));
        }
    }
}