using System.Collections.Generic;
using UnityEngine;

namespace Elias.Scripts.Minigames
{
    public class PatternManager : MonoBehaviour
    {
        public List<int[,]> predefinedPatterns; // List of predefined patterns

        private int _lastPatternIndex = -1;

        private void Start()
        {
            predefinedPatterns = new List<int[,]>
            {
                new int[3, 3] {{ 1, 3, 1 },
                               { 3, 4, 3 },
                               { 1, 3, 1 }},
                
                /*new int[3, 3] {{ 1, 3, 1 },
                               { 1, 4, 3 },
                               { 0, 3, 1 }},
                
                new int[3, 3] {{ 1, 3, 0 },
                               { 0, 3, 0 },
                               { 0, 3, 0 }},
                
                new int[3, 3] {{ 0, 1, 1 },
                               { 3, 3, 2 },
                               { 1, 3, 1 }},
                
                new int[3, 3] {{ 1, 1, 0 },
                               { 2, 1, 3 },
                               { 1, 2, 1 }},
                
                new int[3, 3] {{ 1, 2, 1 },
                               { 3, 1, 2 },
                               { 0, 0, 0 }},*/
                // Add more patterns as needed
            };
        }

        public int[,] GetRandomPattern()
        {
            int newPatternIndex;
            do
            {
                newPatternIndex = Random.Range(0, predefinedPatterns.Count);
            } while (newPatternIndex == _lastPatternIndex);

            _lastPatternIndex = newPatternIndex;
            return predefinedPatterns[newPatternIndex];
        }
    }
}
