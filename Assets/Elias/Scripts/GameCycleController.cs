using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Elias.Scripts
{
    public class GameCycleController : MonoBehaviour
    {
        private int _activeModules;
        private float _timer = 0f;
        private List<Module> _modules;
        
        public DifficultyParameters difficultyParameters;

        public GameCycleController(List<Module> modules)
        {
            _modules = modules;
        }

        public class Module
        {
            public bool isActive;
        }

        private void Start()
        {
            _timer = difficultyParameters.waveInterval;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _timer = difficultyParameters.waveInterval;

                StartCoroutine(ActivateModulesWave());
            }
        }

        private IEnumerator<WaitForSeconds> ActivateModulesWave()
        {
            float waveTimer = 0f;

            while (waveTimer < difficultyParameters.waveDuration)
            {
                yield return new WaitForSeconds(Random.Range(3f,5f));
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
                if (module.isActive)
                {
                    _activeModules++;
                }
            }
        }

        private void ActivateRandomModule()
        {
            if (_activeModules < difficultyParameters.activeModulesLimit)
            {
                List<Module> inactiveModules = new List<Module>();
                foreach (Module module in _modules)
                {
                    if (!module.isActive)
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
            module.isActive = true;
            Debug.Log("Module activated!");
        }
    }
}
