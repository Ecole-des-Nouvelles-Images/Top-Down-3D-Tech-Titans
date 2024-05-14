using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Christopher.Scripts;

namespace Elias.Scripts
{
    public class GameCycleController : MonoBehaviour
    {
        private int _activeModules;
        private float _timer = 0f;
        private List<Module> _modules = new List<Module>();
        
        public List<DifficultyParameters> difficulties;

        private void Start()
        {
            _timer = difficulties[0].waveInterval;
            
            // Populate _modules list
            Module[] foundModules = FindObjectsOfType<Module>();
            foreach (Module module in foundModules)
            {
                _modules.Add(module);
            }
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _timer = difficulties[0].waveInterval;

                StartCoroutine(ActivateModulesWave());
            }
        }

        private IEnumerator<WaitForSeconds> ActivateModulesWave()
        {
            float waveTimer = 0f;

            while (waveTimer < difficulties[0].waveDuration)
            {
                yield return new WaitForSeconds(Random.Range(3f, 5f));
                CountActiveModules();
                ActivateRandomModule();
                waveTimer += 5f;
            }
        }

        private void CountActiveModules()
        {
            _activeModules = 0;
            foreach (Module module in _modules)
            {
                if (module.IsActivated)
                {
                    _activeModules++;
                }
            }
        }

        public int ReturnActiveModules()
        {
            return _activeModules;
        }

        private void ActivateRandomModule()
        {
            if (_activeModules < difficulties[0].activeModulesLimit)
            {
                List<Module> inactiveModules = new List<Module>();
                foreach (Module module in _modules)
                {
                    if (!module.IsActivated)
                    {
                        inactiveModules.Add(module);
                    }
                }

                if (inactiveModules.Count > 0)
                {
                    Module randomModule = inactiveModules[Random.Range(0, inactiveModules.Count)];
                    ActivateModule(randomModule);
                }
            }
        }

        private void ActivateModule(Module module)
        {
            module.Activate();
            Debug.Log("Module activated!");
        }

        // Method to update difficulty parameters based on Phase1Value
        private void UpdateDifficulty(int phase1Value)
        {
            if (phase1Value >= 1 && phase1Value <= 3 && phase1Value <= difficulties.Count)
            {
                DifficultyParameters selectedDifficulty = difficulties[phase1Value - 1]; // Difficulty index starts from 0
                // Update timer and active modules limit based on the selected difficulty
                _timer = selectedDifficulty.waveInterval;
                foreach (Module module in _modules)
                {
                    module.IsActivated = false; // Deactivate all modules
                }
                // Start new wave with updated difficulty
                StartCoroutine(ActivateModulesWave());
            }
            else
            {
                Debug.LogWarning("Invalid Phase1Value or Difficulty Parameters not set for this phase.");
            }
        }
    }
}
