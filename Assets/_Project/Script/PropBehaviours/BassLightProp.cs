using System;
using DG.Tweening;
using UnityEngine;

namespace PropBehaviours
{
    public class BassLightProp : IPropUnit
    {
        [SerializeField] private Light _light;
        private float startIntencity;
        private Tween _tween;
        private void Awake()
        {
            _light = GetComponentInChildren<Light>();
            startIntencity = _light.intensity;
            _tween = _light.DOIntensity(0, 0.2f);
        }
        
        private void Start()
        {
            BassDetector.OnBassDetected += ToggleLight;
        }
        private void ToggleLight()
        {
            _light.intensity = startIntencity;
           
            _tween = _light.DOIntensity(0, 0.2f);
        }
        private void OnDestroy()
        {
            BassDetector.OnBassDetected -= ToggleLight;
        }
    }
}