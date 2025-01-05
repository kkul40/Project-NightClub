using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class UIToggleButton : MonoBehaviour
    {
        [SerializeField] private Image CheckMarkImage;
        private Button _button;
        private void Awake()
        {
            CheckMarkImage.raycastTarget = false;
            _button = GetComponent<Button>();
        }

        public void ToggleCheckMark(bool toggle)
        {
            CheckMarkImage.enabled = toggle;
        }

        public void AddToggleListener(UnityAction action)
        {
            _button.onClick.AddListener(action);
        }
    }
}