using System;
using UnityEngine;

namespace Elias.Scripts.Scripts
{
    public class LightColorAnimation: MonoBehaviour
    {
        public Color CurrentColor => Light.color;

        public Color OriginalColor { get; private set; }
        
        public Light Light {get; private set; }

        private void Awake()
        {
            Light = GetComponent<Light>();
            OriginalColor = Light.color;
        }
    }
}