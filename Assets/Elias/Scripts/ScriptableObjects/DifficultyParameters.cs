using UnityEngine;
using Unity.Mathematics;

namespace Elias.Scripts
{
    [CreateAssetMenu(fileName = "DifficultyParameters", menuName = "Game/DifficultyParameters")]
    public class DifficultyParameters : ScriptableObject
    {
        public float2 waveDurationRange;
        public float2 waveIntervalRange;
        public float2 breachIntervalRange;
        public int activeModulesLimit;
        public float initialDelay;

        // Store original values to reset later
        private float2 _originalWaveDurationRange;
        private float2 _originalWaveIntervalRange;
        private float2 _originalBreachIntervalRange;
        private int _originalActiveModulesLimit;
        private float _originalInitialDelay;

        private void OnEnable()
        {
            _originalWaveDurationRange = waveDurationRange;
            _originalWaveIntervalRange = waveIntervalRange;
            _originalBreachIntervalRange = breachIntervalRange;
            _originalActiveModulesLimit = activeModulesLimit;
            _originalInitialDelay = initialDelay;
        }

        public void ResetValues()
        {
            waveDurationRange = _originalWaveDurationRange;
            waveIntervalRange = _originalWaveIntervalRange;
            breachIntervalRange = _originalBreachIntervalRange;
            activeModulesLimit = _originalActiveModulesLimit;
            initialDelay = _originalInitialDelay;
        }

        public float GetRandomWaveDuration()
        {
            return UnityEngine.Random.Range(waveDurationRange.x, waveDurationRange.y);
        }

        public float GetRandomWaveInterval()
        {
            return UnityEngine.Random.Range(waveIntervalRange.x, waveIntervalRange.y);
        }

        public float GetRandomBreachInterval()
        {
            return UnityEngine.Random.Range(breachIntervalRange.x, breachIntervalRange.y);
        }
    }
}