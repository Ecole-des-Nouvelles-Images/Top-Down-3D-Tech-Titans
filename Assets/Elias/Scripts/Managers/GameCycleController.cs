using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;
using Elias.Scripts.Minigames;
using Random = UnityEngine.Random;

namespace Elias.Scripts.Managers
{
    public class GameCycleController : MonoBehaviour
    {
        public int activeModules;

        private List<BreachModule> _modules = new List<BreachModule>();
        public List<DifficultyParameters> difficulties;
        private bool _isDifficultySet;
        private bool _hasUpdatedDifficulty;
        private int _activeDifficulty;
        public bool noActiveBreach;

        private float _intervalTimer;

        private void Start()
        {
            // Reset all difficulty parameters at the start of the game
            foreach (var difficulty in difficulties)
            {
                if (difficulty != null)
                {
                    difficulty.ResetValues();
                }
                else
                {
                    Debug.LogWarning("A DifficultyParameters instance is null.");
                }
            }
        }

        private void Update()
        {
            if (!_hasUpdatedDifficulty || !_isDifficultySet)
            {
                return;
            }

            var currentDifficulty = difficulties[_activeDifficulty];
            if (currentDifficulty == null)
            {
                Debug.LogError("Current difficulty is null.");
                return;
            }

            Debug.Log(currentDifficulty.initialDelay);

            currentDifficulty.initialDelay -= Time.deltaTime;
            _intervalTimer -= Time.deltaTime;

            if (currentDifficulty.initialDelay <= 0f && _intervalTimer <= 0f)
            {
                StartCoroutine(ActivateModulesWave());
                _intervalTimer = currentDifficulty.GetRandomWaveInterval();
            }
        }

        private IEnumerator ActivateModulesWave()
        {
            var currentDifficulty = difficulties[_activeDifficulty];
            if (currentDifficulty == null)
            {
                yield break;
            }

            float waveTimer = 0f;
            float waveDuration = currentDifficulty.GetRandomWaveDuration();

            while (waveTimer < waveDuration)
            {
                waveTimer += Time.deltaTime;

                yield return new WaitForSeconds(Random.Range(3f, 5f));

                FindModules();
                CountActiveBreach();
                ActivateRandomModule();
            }
        }

        public void FindModules()
        {
            _modules.Clear();

            BreachModule[] foundModules = FindObjectsOfType<BreachModule>();
            foreach (BreachModule module in foundModules)
            {
                _modules.Add(module);
            }
        }

        public void CountActiveBreach()
        {
            activeModules = 0;
            foreach (BreachModule module in _modules)
            {
                if (module.IsActivated)
                {
                    activeModules++;
                }
            }

            noActiveBreach = activeModules == 0;
        }

        private void ActivateRandomModule()
        {
            var currentDifficulty = difficulties[_activeDifficulty];
            if (currentDifficulty == null)
            {
                return;
            }

            if (activeModules < currentDifficulty.activeModulesLimit)
            {
                List<SubmarinModule> inactiveModules = new List<SubmarinModule>();
                foreach (SubmarinModule module in _modules)
                {
                    if (!module.IsActivated)
                    {
                        inactiveModules.Add(module);
                    }
                }

                if (inactiveModules.Count > 0)
                {
                    SubmarinModule randomModule = inactiveModules[Random.Range(0, inactiveModules.Count)];
                    ActivateModule(randomModule);
                }
            }
        }

        private void ActivateModule(SubmarinModule module)
        {
            module.Activate();
            Debug.Log("Module activated!");
        }

        public void UpdateDifficulty(int phase1Value)
        {
            if (difficulties == null || difficulties.Count == 0)
            {
                Debug.LogWarning("Difficulty parameters not set.");
                return;
            }

            if (phase1Value >= 1 && phase1Value <= difficulties.Count)
            {
                DifficultyParameters selectedDifficulty = difficulties[phase1Value - 1];
                if (selectedDifficulty == null)
                {
                    Debug.LogError("Selected difficulty is null.");
                    return;
                }

                selectedDifficulty.ResetValues(); // Reset to initial values
                _intervalTimer = selectedDifficulty.GetRandomWaveInterval();
                _activeDifficulty = phase1Value - 1;
                foreach (SubmarinModule module in _modules)
                {
                    module.IsActivated = false;
                }
                _hasUpdatedDifficulty = true;
                _isDifficultySet = true;
            }
            else
            {
                Debug.LogWarning("Invalid Phase1Value or Difficulty Parameters not set for this phase.");
            }
        }
    }
}
