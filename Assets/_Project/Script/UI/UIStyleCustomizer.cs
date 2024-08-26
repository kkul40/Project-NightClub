using System;
using Data;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UIStyleCustomizer : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private UIType _uiType;

        private void Awake()
        {
            UpdateVisual(InitConfig.Instance.GetDefaultUIStyle);
        }

        public void UpdateVisual(UIStyleSo uiStyleSo)
        {
            _image = GetComponent<Image>();

            switch (_uiType)
            {
                case UIType.Window:
                    _image.color = uiStyleSo.WindowColor;
                    break;
                case UIType.InnerWindow:
                    _image.color = uiStyleSo.InnerWindowColor;
                    break;
                case UIType.Button:
                    _image.color = uiStyleSo.ButtonColor;
                    break;
                default:
                    Debug.LogError("UIType is Null : " + _uiType.ToString());
                    break;
            }
        }
    }

    public enum UIType
    {
        Window,
        InnerWindow,
        Button,
    }
}