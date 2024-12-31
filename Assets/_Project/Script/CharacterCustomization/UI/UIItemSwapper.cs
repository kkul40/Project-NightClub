using System;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterCustomization.UI
{
    public class UIItemSwapper : MonoBehaviour
    {
        [SerializeField] private Button m_LeftButton;
        [SerializeField] private Button m_RightButton;

        public Action OnClickLeft, OnClickRight;

        private void Awake()
        {
            m_LeftButton.onClick.AddListener(_OnClickLeftButton);
            m_RightButton.onClick.AddListener(_OnClickRightButton);
        }

        private void _OnClickLeftButton()
        {
            OnClickLeft?.Invoke();
        }

        private void _OnClickRightButton()
        {
            OnClickRight?.Invoke();
        }
    }
}
