using System.Collections.Generic;
using System.Music;
using Data;
using DG.Tweening;
using Disco_Building;
using DiscoSystem;
using UnityEngine;

namespace PropBehaviours.LightBehaviours
{
    public class BassSpeakerProp : IPropUnit
    {
        [Header("Settings")]
        public float scaleFactor = 1.05f;
        public float moveFactor = 0.025f; 
        public List<Transform> _animatedParts;
        
        private List<Vector3> startPositions = new List<Vector3>();
        private List<Tween> _tweenScales = new List<Tween>();
        private List<Tween> _tweenMoves = new List<Tween>();

        private void Awake()
        {
            for (int i = 0; i < _animatedParts.Count; i++)
            {
                startPositions.Add(_animatedParts[i].localPosition);
                _tweenScales.Add(null);
                _tweenMoves.Add(null);
            }
        }

        private void Start()
        {
            BassDetector.OnBassDetected += BassSpeaker;
        }

        private void BassSpeaker()
        {
            for (int i = 0; i < _animatedParts.Count; i++)
            {
                _animatedParts[i].localScale = Vector3.one * scaleFactor;
                _animatedParts[i].localPosition = startPositions[i] + (_animatedParts[i].localRotation * Vector3.forward * moveFactor);
            
                if (_tweenMoves[i] != null)
                {
                    _tweenScales[i].Kill();
                    _tweenMoves[i].Kill();
                }
           
                _tweenScales[i] = _animatedParts[i].DOScale(Vector3.one, 0.2f);
                _tweenMoves[i] = _animatedParts[i].DOLocalMove(startPositions[i], 0.2f);
            }
        }
        
        private void OnDestroy()
        {
            BassDetector.OnBassDetected -= BassSpeaker;
        }
    }
}