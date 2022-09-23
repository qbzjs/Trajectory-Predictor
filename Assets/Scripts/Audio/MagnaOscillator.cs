using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnaOscillator : MonoBehaviour
{
    public TargetMagnitudeTracker magnitudeTracker;
    
    class Oscillator
    {
        public float magnitude { get; set; }

        public float Frequency {
            set { _targetDelta = value / 48000; }
        }

        public float Amplitude { get; set; }
        public float Modulation { get; set; }

        float _targetDelta;
        float _delta;
        float _phase;

        public float Tick()
        {
            _delta = Mathf.Lerp(_delta, _targetDelta, 0.05f);
            _phase = (_phase + _delta) % 1;
            var p = Mathf.PI * 2 * _phase;
            return Mathf.Sin(p + Modulation * Mathf.Sin(p * 7)) * Amplitude;
        }
    }

    Oscillator oscillator;
    AudioClip clip;

    void Start()
    {
        oscillator = new Oscillator();
        
        clip = AudioClip.Create("Test", 0x7fffffff, 1, 48000, true, OnPcmRead);

        var source = GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }

    
    void Update()
    {
        if (magnitudeTracker)
        {
            oscillator.Frequency = 55.0f * Mathf.Pow(2, magnitudeTracker.feedbackAmplitude * 4);
            oscillator.Amplitude = Mathf.Clamp01(magnitudeTracker.feedbackAmplitude * 5);
            oscillator.Modulation = magnitudeTracker.feedbackAmplitude;
        }
    }
    
    void OnPcmRead(float[] data)
    {
    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        for (var i = 0; i < data.Length; i++)
        {
            var v = 0.0f;
            v += oscillator.Tick();

            data[i] = v;
        }
    }
}
