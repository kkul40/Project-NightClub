using System;
using DG.Tweening;
using DiscoSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PropBehaviours
{
    public class BassLightProp : IPropUnit
    {
        
        [SerializeField] private Light _light;
        private float startIntencity;
        private Tween _tween;

        private float time;
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
            time = Time.time;
            float cycleSpeed = 0.25f;
            
            float r = Mathf.Sin(time * cycleSpeed) * 0.5f + 0.5f; // Sine outputs [-1, 1], normalize to [0, 1]
            float g = Mathf.Sin(time * cycleSpeed + Mathf.PI * 2 / 3) * 0.5f + 0.5f; // Offset by 120 degrees
            float b = Mathf.Sin(time * cycleSpeed + Mathf.PI * 4 / 3) * 0.5f + 0.5f; // Offset by 240 degrees

            // Set the color
            Color newColor = new Color(r, g, b);
            _light.color = newColor;
            _light.intensity = startIntencity;

            if (_tween != null)
            {
                _tween.Kill();
            }
           
            _tween = _light.DOIntensity(0, 0.2f);
        }
        private void OnDestroy()
        {
            BassDetector.OnBassDetected -= ToggleLight;
        }
    }
}