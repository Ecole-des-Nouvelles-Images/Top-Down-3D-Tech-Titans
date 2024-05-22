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
        private List<SubmarinModule> _modules = new List<SubmarinModule>();

        public List<DifficultyParameters> difficulties;

        public void InitializeGameCycle(int phase1Value)
        {
            UpdateDifficulty(phase1Value);

            if (difficulties == null || difficulties.Count == 0)
            {
                Debug.LogWarning("No difficulty parameters set.");
                return;
            }

            _timer = difficulties[0].waveInterval;

            SubmarinModule[] foundModules = FindObjectsOfType<SubmarinModule>();
            foreach (SubmarinModule module in foundModules)
            {
                _modules.Add(module);
            }
        }

        private void Update()
        {
            if (difficulties == null || difficulties.Count == 0)
            {
                return;
            }

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
            foreach (SubmarinModule module in _modules)
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

        private void UpdateDifficulty(int phase1Value)
        {
            if (difficulties == null || difficulties.Count == 0)
            {
                Debug.LogWarning("Difficulty parameters not set.");
                return;
            }

            if (phase1Value >= 1 && phase1Value <= 3 && phase1Value <= difficulties.Count)
            {
                DifficultyParameters selectedDifficulty = difficulties[phase1Value - 1];
                _timer = selectedDifficulty.waveInterval;
                foreach (SubmarinModule module in _modules)
                {
                    module.IsActivated = false;
                }
                StartCoroutine(ActivateModulesWave());
            }
            else
            {
                Debug.LogWarning("Invalid Phase1Value or Difficulty Parameters not set for this phase.");
            }
        }
    }
}
