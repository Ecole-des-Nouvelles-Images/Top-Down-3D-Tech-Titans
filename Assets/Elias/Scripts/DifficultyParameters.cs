using UnityEngine;

namespace Elias.Scripts
{
    [CreateAssetMenu(fileName = "DifficultyParameters", menuName = "Game/DifficultyParameters")]
    public class DifficultyParameters : ScriptableObject
    {
        public float waveDuration;
        public float waveInterval;
        public int activeModulesLimit;
    }
}