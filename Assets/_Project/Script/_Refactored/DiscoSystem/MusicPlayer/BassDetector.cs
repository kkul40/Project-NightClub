using System;
using UnityEngine;

namespace DiscoSystem
{
    [Serializable]
    public class BassDetector
    {
        public AudioSource audioSource;
        
        public float Sensitivity = 0.8f;
        public float Threshold = 0.5f;
        private float[] _spectrumData;
        private float _previousBassAmplitude = 0;
        
        public static Action OnBassDetected;
        
        public BassDetector(AudioSource audioSource)
        {
            this.audioSource = audioSource;
            _spectrumData = new float[512];
        }
        
        public void UpdateDetector()
        {
            audioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.Blackman);
            float bassAmplitude = 0;
            
            int bassRange = Mathf.Min(20, _spectrumData.Length); 
            for (int i = 0; i < bassRange; i++)
                bassAmplitude += _spectrumData[i];
            
            bassAmplitude *= Sensitivity;

            if (bassAmplitude > Threshold && bassAmplitude > _previousBassAmplitude)
                OnBassDetected?.Invoke();
            
            _previousBassAmplitude = bassAmplitude;
        }
    }
}