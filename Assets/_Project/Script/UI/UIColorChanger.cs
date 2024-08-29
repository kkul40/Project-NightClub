using System;
using Data;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIColorChanger : MonoBehaviour
    {
        [SerializeField] private eUIType _uiType;
        private void Start()
        {
            UpdateUIColor(InitConfig.Instance.GetDefaultUIColorStyle);
        }

        public void UpdateUIColor(UIColorStyle colorStyle)
        {
            var image = GetComponent<Image>();

            Color newColor = Color.clear;
            switch (_uiType)
            {
                case eUIType.Window:
                    newColor = colorStyle.UIWindowColor;
                    break;
                case eUIType.InnerWindow:
                    newColor = colorStyle.UIInnerWindowColor;
                    break;
                case eUIType.Button:
                    newColor = colorStyle.UIButtonCollor;
                    break;
            }

            image.color = newColor;
        }
    }

    public enum eUIType
    {
        Window,
        InnerWindow,
        Button,
    }
}