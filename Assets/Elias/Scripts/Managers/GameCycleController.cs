using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;
using Elias.Scripts.Minigames;
using UnityEngine.Serialization;

namespace Elias.Scripts.Managers
{
    public class GameCycleController : MonoBehaviour
    {
        public int activeModules;
        public float timer = 10f;
        private List<BreachModule> _modules = new List<BreachModule>();
        public List<DifficultyParameters> difficulties;
        private bool _isDifficultySet;
        private bool _hasUpdatedDifficulty;
        private int _activeDifficulty;
        

        public bool noActiveBreach;

        private void Update()
        {
            if (!_hasUpdatedDifficulty)
            {
                return;
            }

            if (!_isDifficultySet)
            {
                if (difficulties != null && difficulties.Count > 0)
                {
                    _isDifficultySet = true;
                }
                return;
            }

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = difficulties[_activeDifficulty].waveInterval;

                StartCoroutine(ActivateModulesWave());
            }
        }

        private IEnumerator<WaitForSeconds> ActivateModulesWave()
        {
            float waveTimer = 0f;

            while (waveTimer < difficulties[_activeDifficulty].waveDuration)
            {
                yield return new WaitForSeconds(Random.Range(3f, 5f));

                FindModules();
                CountActiveBreach();
                ActivateRandomModule();
                waveTimer += 5f;
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

            if (activeModules == 0)
            {
                noActiveBreach = true;
            }
            else
            {
                noActiveBreach = false;
            }
        }

        /*public int ReturnActiveModules()
        {
            return _activeModules;
        }*/

        private void ActivateRandomModule()
        {
            if (activeModules < difficulties[_activeDifficulty].activeModulesLimit)
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

            if (phase1Value >= 1 && phase1Value <= 3 && phase1Value <= difficulties.Count)
            {
                DifficultyParameters selectedDifficulty = difficulties[phase1Value - 1];
                timer = selectedDifficulty.waveInterval;
                _activeDifficulty = phase1Value - 1;
                foreach (SubmarinModule module in _modules)
                {
                    module.IsActivated = false;
                }
                _hasUpdatedDifficulty = true;
                StartCoroutine(ActivateModulesWave());
            }
            else
            {
                Debug.LogWarning("Invalid Phase1Value or Difficulty Parameters not set for this phase.");
            }
        }
    }
}
