using UnityEngine;

namespace Elias.Scripts.Light
{
    public class LightColorAnimation: MonoBehaviour
    {
        public Color CurrentColor => Light.color;

        public Color OriginalColor { get; private set; }
        
        public UnityEngine.Light Light {get; private set; }

        private void Awake()
        {
            Light = GetComponent<UnityEngine.Light>();
            OriginalColor = Light.color;
        }
    }
}