using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Elias.Scripts.Minigames
{
    public class SkillCheckWarframe : SubmarinModule
    {
        public bool playerInteracting;
        public GameObject canvas;
        public GameObject squareList; // Reference to the SquareList GameObject
        public List<GameObject> squarePrefabs; // List of prefab variants
        public Transform gridTransform; // Reference to the Grid GameObject with GridLayoutGroup

        private List<PatternSquare> _patternSquares;
        private int _selectedIndex;
        public bool IsPatternValid => ValidatePattern();

        private void Start()
        {
            InitializePatternSquares();
            _selectedIndex = 0; // Start with the first square selected
            HighlightSelectedSquare();
        }

        private void InitializePatternSquares()
        {
            _patternSquares = new List<PatternSquare>();
            
            foreach (Transform child in gridTransform)
            {
                Destroy(child.gameObject);
            }

            int[,] predefinedPattern = new int[3, 3]
            {
                { 1, 3, 1 },
                { 3, 4, 3 },
                { 1, 3, 1 }
            };
            
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int prefabIndex = predefinedPattern[row, col];
                    var squareInstance = Instantiate(squarePrefabs[prefabIndex], gridTransform);
                    var patternSquare = squareInstance.GetComponent<PatternSquare>();
                    if (patternSquare != null)
                    {
                        _patternSquares.Add(patternSquare);
                    }
                    squareInstance.GetComponent<Image>().enabled = true;
                }
            }
        }

        private void HighlightSelectedSquare()
        {
            for (int i = 0; i < _patternSquares.Count; i++)
            {
                var image = _patternSquares[i].GetComponent<Image>();
                image.color = (i == _selectedIndex) ? Color.yellow : Color.white; // Highlight selected square
            }
        }

        void Update()
        {
            if (playerInteracting)
            {
                canvas.SetActive(true);
            }
            else
            {
                canvas.SetActive(false);
            }
        }

        bool ValidatePattern()
        {
            for (int i = 0; i < _patternSquares.Count; i++)
            {
                PatternSquare currentSquare = _patternSquares[i];

                if (i % 3 != 2 && !currentSquare.IsConnected(_patternSquares[i + 1], PatternSquare.Direction.Right)) return false; // Check right
                if (i % 3 != 0 && !currentSquare.IsConnected(_patternSquares[i - 1], PatternSquare.Direction.Left)) return false;  // Check left
                if (i / 3 != 2 && !currentSquare.IsConnected(_patternSquares[i + 3], PatternSquare.Direction.Down)) return false;  // Check bottom
                if (i / 3 != 0 && !currentSquare.IsConnected(_patternSquares[i - 3], PatternSquare.Direction.Up)) return false;   // Check top
            }
            return true;
        }

        /*void UpdateStateDisplayObjects(Material material)
        {
            foreach (var obj in StateDisplayObject)
            {
                obj.GetComponent<Renderer>().material = material;
            }
        }*/

        public override void Activate()
        {
            IsActivated = true;
            State = 1;
        }

        public override void Deactivate()
        {
            IsActivated = false;
            State = 0;
        }

        public override void Interact(GameObject playerUsingModule)
        {
            playerInteracting = true;
            Activate();
        }

        public override void StopInteract()
        {
            playerInteracting = false;
            PlayerUsingModule = null;
        }

        public override void Validate()
        {
            _patternSquares[_selectedIndex].RotateSquare();

            if (IsPatternValid)
            {
                Debug.Log("Pattern matched!");
                Succes.Add(true);
                //UpdateStateDisplayObjects(StatesMaterials[1]);
                Deactivate();
                canvas.SetActive(false);
            }
            else
            {
                Debug.Log("Pattern not matched!");
                Succes.Add(false);
                //UpdateStateDisplayObjects(StatesMaterials[0]);
            }
        }

        public override void NavigateX(float moveX) { }

        public override void NavigateY(float moveY) { }

        public override void Up()
        {
            int newIndex = _selectedIndex - 3;
            if (newIndex >= 0)
            {
                _selectedIndex = newIndex;
                HighlightSelectedSquare();
            }
        }

        public override void Down()
        {
            int newIndex = _selectedIndex + 3;
            if (newIndex < _patternSquares.Count)
            {
                _selectedIndex = newIndex;
                HighlightSelectedSquare();
            }
        }

        public override void Left()
        {
            int newIndex = _selectedIndex - 1;
            if (_selectedIndex % 3 != 0 && newIndex >= 0)
            {
                _selectedIndex = newIndex;
                HighlightSelectedSquare();
            }
        }

        public override void Right()
        {
            int newIndex = _selectedIndex + 1;
            if (_selectedIndex % 3 != 2 && newIndex < _patternSquares.Count)
            {
                _selectedIndex = newIndex;
                HighlightSelectedSquare();
            }
        }
    }
}
