using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Emotes
{
    public class UIEmote : MonoBehaviour
    {
        private SpriteRenderer _image;
        private Transform _followTarget;
        [SerializeField] private Vector3 Offset;

        private Tween _tween;

        private void Awake()
        {
            _image = GetComponent<SpriteRenderer>();
            transform.LookAt(Camera.main.transform, Vector3.up);
        }

        public void Init(Sprite sprite, Transform followTarget)
        {
            _image.sprite = sprite;
            _followTarget = followTarget;
            
            transform.position = _followTarget.position + Offset;
            _tween = transform.DOMove(_followTarget.position + Offset + (Vector3.up * 0.2f), 1f).SetEase(Ease.OutExpo).SetLink(gameObject);
        }
    }
}