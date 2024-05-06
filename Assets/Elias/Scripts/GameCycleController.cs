using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Elias.Scripts
{
    public class GameCycleController : MonoBehaviour
    {
        private int _activeModules; // compte le nombre de module deja actif
        private int _activeModulesLimit = 3; // limite de brèche en fonction de la difficulté
        private float _timer = 0f;
        private List<Module> _modules;

        private class Module
        {
            public bool isActive;
        }

        private void Start()
        {
            _timer = 60f;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _timer = 60f;

                CountActiveModules();
                ActivateRandomModule();
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
            if (_activeModules < _activeModulesLimit)
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
