using System;
using System.ObjectPooling;
using DG.Tweening;
using UnityEngine;

namespace UI.Emotes
{
    public class UIEmote : MonoBehaviour, IPoolable
    {
        public Transform mTransform { get; set; }
        public bool IsActive { get; set; }
        public float RemainingTime { get; set; }
        public ObjectPooler Pool { get; set; }
        
        private SpriteRenderer _image;
        private Transform _followTarget;
        [SerializeField] private Vector3 Offset;

        private Transform camTransform;

        private Tween _tween;

        private void Awake()
        {
            mTransform = transform;
            _image = GetComponent<SpriteRenderer>();
            camTransform = Camera.main.transform;
        }

        public void Init(Sprite sprite, Transform followTarget)
        {
            _image.sprite = sprite;
            _followTarget = followTarget;
            
            transform.LookAt(transform.position + camTransform.transform.rotation * Vector3.forward, camTransform.transform.rotation * Vector3.up);
            transform.position = _followTarget.position + Offset;
            _tween = transform.DOMoveY(transform.position.y + 0.2f, 1f).SetEase(Ease.OutElastic).SetLink(gameObject);
        }

        private void Update()
        {
            if (_followTarget == null)
            {
                ((IPoolable)this).ReturnToPool();
                return;
            }
            
            transform.LookAt(transform.position + camTransform.transform.rotation * Vector3.forward, camTransform.transform.rotation * Vector3.up);
            Vector3 nextPos = _followTarget.position + Offset;
            transform.position = new Vector3(nextPos.x, transform.position.y, nextPos.z);
        }
    }
}