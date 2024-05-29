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
        public float2 generatorRange;
        
        public int activeModulesLimit;
        public float initialDelay;
        
        public int generatorCountLimit;

        private float2 _originalWaveDurationRange;
        private float2 _originalWaveIntervalRange;
        private float2 _originalBreachIntervalRange;
        private float2 _originalGeneratorRange;
        
        private int _originalActiveModulesLimit;
        private float _originalInitialDelay;
        
        private int _originalGeneratorCountLimit;

        private void OnEnable()
        {
            _originalWaveDurationRange = waveDurationRange;
            _originalWaveIntervalRange = waveIntervalRange;
            _originalBreachIntervalRange = breachIntervalRange;
            _originalGeneratorRange = generatorRange;
            
            _originalActiveModulesLimit = activeModulesLimit;
            _originalInitialDelay = initialDelay;

            _originalGeneratorCountLimit = generatorCountLimit;
        }

        public void ResetValues()
        {
            waveDurationRange = _originalWaveDurationRange;
            waveIntervalRange = _originalWaveIntervalRange;
            breachIntervalRange = _originalBreachIntervalRange;
            generatorRange = _originalGeneratorRange;
            
            activeModulesLimit = _originalActiveModulesLimit;
            initialDelay = _originalInitialDelay;

            generatorCountLimit = _originalGeneratorCountLimit;
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
        
        public float GetRandomGeneratorRange()
        {
            return UnityEngine.Random.Range(generatorRange.x, generatorRange.y);
        }
    }
}