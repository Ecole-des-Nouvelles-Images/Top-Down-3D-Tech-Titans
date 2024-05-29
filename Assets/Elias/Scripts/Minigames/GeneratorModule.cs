using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using Christopher.Scripts.Modules;
using Elias.Scripts.Managers;
using Elias.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Elias.Scripts.Minigames
{
    public class GeneratorModule : SubmarinModule
    {
        public bool playerInteracting;
        public GameObject canvas;
        public GameObject squareList; // Reference to the SquareList GameObject
        public List<GameObject> squarePrefabs; // List of prefab variants
        public Transform gridTransform; // Reference to the Grid GameObject with GridLayoutGroup
        [SerializeField] private GameObject greenLights;
        private List<PatternSquare> _patternSquares;
        private int _selectedIndex;
        public bool IsPatternValid => ValidatePattern();

        private PatternManager _patternManager; // Reference to the PatternManager

        private void Start()
        {
            IsActivated = true;
            _patternManager = GetComponent<PatternManager>(); // Assuming PatternManager is on the same GameObject
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

        private void HighlightSelectedSquare() {
            for (int i = 0; i < _patternSquares.Count; i++) {
                var image = _patternSquares[i].GetComponent<Image>();
                image.color = (i == _selectedIndex) ? Color.yellow : Color.white; // Highlight selected square
            }
        }

        void Update() {
            
            if (IsActivated) {
                State = 1;
                greenLights.SetActive(true);
                
                List<SubmarinModule> foundModules = new List<SubmarinModule>(FindObjectsOfType<SubmarinModule>());
                foreach (SubmarinModule module in foundModules)
                {
                    switch (module.GetType().Name)
                    {
                        case nameof(ScreenModule):
                            module.IsActivated = true;
                            break;
                        case nameof(BoosterModule):
                            module.IsActivated = true;
                            break;
                        case nameof(FixingDrillModule):
                            module.IsActivated = true;
                            break;
                        case nameof(PressureModule):
                            module.IsActivated = true;
                            break;
                        case nameof(StorageTorpedo):
                            module.IsActivated = true;
                            break;
                        case nameof(TorpedoLauncherModule):
                            module.IsActivated = true;
                            break;
                        case nameof(HatchesModule):
                            module.IsActivated = true;
                            break;
                        case nameof(OxygenModule):
                            module.IsActivated = true;
                            break;
                        case nameof(StorageCapsules):
                            module.IsActivated = true;
                            break;
                    }
                }
            }
            
            else if (!IsActivated) {
                State = 0;
                greenLights.SetActive(false);
                
                List<SubmarinModule> foundModules = new List<SubmarinModule>(FindObjectsOfType<SubmarinModule>());
                foreach (SubmarinModule module in foundModules)
                {
                    switch (module.GetType().Name)
                    {
                        case nameof(ScreenModule):
                            module.IsActivated = false;
                            break;
                        case nameof(BoosterModule):
                            module.IsActivated = false;
                            break;
                        case nameof(FixingDrillModule):
                            module.IsActivated = false;
                            break;
                        case nameof(PressureModule):
                            module.IsActivated = false;
                            break;
                        case nameof(StorageTorpedo):
                            module.IsActivated = false;
                            break;
                        case nameof(TorpedoLauncherModule):
                            module.IsActivated = false;
                            break;
                        case nameof(HatchesModule):
                            module.IsActivated = false;
                            break;
                        case nameof(OxygenModule):
                            module.IsActivated = false;
                            break;
                        case nameof(StorageCapsules):
                            module.IsActivated = false;
                            break;
                    }
                }
            }
            if (playerInteracting)
            {
                canvas.SetActive(true);
            }
            else
            {
                canvas.SetActive(false);
            }

            switch (State)
            {
                case 0 :
                    Material[]mats = StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials;
                    mats[3] = StatesMaterials[State];
                    StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials = mats;
                    break;
                case 1:
                    mats = StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials;
                    mats[3] = StatesMaterials[State];
                    StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials = mats;
                    break;
            }
        }

        bool ValidatePattern()
        {
            for (int i = 0; i < _patternSquares.Count; i++)
            {
                PatternSquare currentSquare = _patternSquares[i];

                if (i % 3 != 2 && !currentSquare.IsConnected(_patternSquares[i + 1], PatternSquare.Direction.Right)) {
                    return false; // Check right
                }
                if (i % 3 != 0 && !currentSquare.IsConnected(_patternSquares[i - 1], PatternSquare.Direction.Left)) {
                    return false;  // Check left
                }
                if (i / 3 != 2 && !currentSquare.IsConnected(_patternSquares[i + 3], PatternSquare.Direction.Down)) {
                    return false;  // Check bottom
                }
                if (i / 3 != 0 && !currentSquare.IsConnected(_patternSquares[i - 3], PatternSquare.Direction.Up)) {
                    return false;   // Check top
                }
            }
            return true;
        }

        public override void Activate() {
            IsActivated = false;
        }

        public override void Deactivate()
        {
            IsActivated = true;
            State = 0;
            canvas.SetActive(false);

            if (PlayerUsingModule != null)
            {
                var playerController = PlayerUsingModule.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.QuitInteraction();
                }
            }
        }


        public override void Interact(GameObject playerUsingModule) {
            if (!IsActivated && PlayerUsingModule == null && playerUsingModule.GetComponent<PlayerController>().MyItem == 2) {
                PlayerUsingModule = playerUsingModule;
                playerInteracting = true;
                InitializePatternSquares();
                PlayerUsingModule.GetComponent<PlayerController>().MyItem = 0;
            }
            else {
                playerUsingModule.GetComponent<PlayerController>().QuitInteraction();
            }
        }

        public override void StopInteract()
        {
            PlayerUsingModule = null;
            playerInteracting = false;
        }


        public override void Validate()
        {
            _patternSquares[_selectedIndex].RotateSquare();

            if (IsPatternValid)
            {
                Debug.Log("Pattern matched!");
                Succes.Add(true);
                StartCoroutine(DeactivateAfterDelay());
            }
            else
            {
                Debug.Log("Pattern not matched!");
                Succes.Add(false);
            }
        }

        private IEnumerator DeactivateAfterDelay()
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            Deactivate();
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
