using UnityEngine;
using Unity.Mathematics;

namespace Elias.Scripts
{
    [CreateAssetMenu(fileName = "DifficultyParameters", menuName = "Game/DifficultyParameters")]
    public class DifficultyParameters : ScriptableObject
    {
        public float2 waveDurationRange; // Min and Max values for waveDuration
        public float2 waveIntervalRange; // Min and Max values for waveInterval
        public int activeModulesLimit;
        public float initialDelay;

        public float GetRandomWaveDuration()
        {
            return UnityEngine.Random.Range(waveDurationRange.x, waveDurationRange.y);
        }

        public float GetRandomWaveInterval()
        {
            return UnityEngine.Random.Range(waveIntervalRange.x, waveIntervalRange.y);
        }
    }
}