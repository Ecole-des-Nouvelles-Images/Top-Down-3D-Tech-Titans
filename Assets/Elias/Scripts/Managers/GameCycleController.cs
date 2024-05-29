using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;
using Elias.Scripts.Minigames;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Elias.Scripts.Managers
{
    public class GameCycleController : MonoBehaviourSingleton<GameCycleController>
    {
        public int activeModules;

        public List<BreachModule> modules = new List<BreachModule>();
        public List<DifficultyParameters> difficulties;
        private bool _isDifficultySet;
        private bool _hasUpdatedDifficulty;
        private int _activeDifficulty;
        public bool noActiveBreach;

        private float _generatorTime;
        private float _waveTimer;
        private float _cooldownTimer;
        private bool _halfCooldown;
        private bool _noCooldown;
        private bool _cooldownAdjusted;
        
        public int activePhase;
        public int generatorEventCount;

        private enum GameState { InitialDelay, Wave, Cooldown }
        private GameState _currentState;

        private void Start()
        {
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

            if (difficulties.Count > 0)
            {
                _currentState = GameState.InitialDelay;
                _waveTimer = difficulties[_activeDifficulty].initialDelay;
            }
            else
            {
                Debug.LogError("No difficulties set.");
            }
        }

        private void Update()
        {
            if (activePhase == 1)
            {
                difficulties = null;
            }

            if (_generatorTime > 0)
            {
                _generatorTime -= Time.deltaTime;
            }

            if (difficulties != null && activePhase != 1 || _generatorTime <= 0 && generatorEventCount != 0)
            {
                _generatorTime = difficulties[_activeDifficulty].GetRandomGeneratorRange();
                
                if (_generatorTime <= 0 && generatorEventCount <= difficulties[_activeDifficulty].generatorCountLimit)
                {
                    generatorEventCount++;
                    
                    GeneratorModule generator = FindObjectOfType<GeneratorModule>();
                    if (generator != null)
                    {
                        generator.IsActivated = false;
                    }

                    if (generatorEventCount < difficulties[_activeDifficulty].generatorCountLimit)
                    {
                        _generatorTime = difficulties[_activeDifficulty].GetRandomGeneratorRange();
                    }
                }
            }
            
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

            switch (_currentState)
            {
                case GameState.InitialDelay:
                    HandleInitialDelay(currentDifficulty);
                    break;
                case GameState.Wave:
                    HandleWave(currentDifficulty);
                    break;
                case GameState.Cooldown:
                    HandleCooldown(currentDifficulty);
                    break;
            }
        }

        private void HandleInitialDelay(DifficultyParameters currentDifficulty)
        {
            _waveTimer -= Time.deltaTime;
            if (_waveTimer <= 0f)
            {
                _currentState = GameState.Wave;
                _waveTimer = currentDifficulty.GetRandomWaveDuration();
                StartCoroutine(ActivateModulesWave());
            }
        }

        private void HandleWave(DifficultyParameters currentDifficulty)
        {
            _waveTimer -= Time.deltaTime;
            if (_waveTimer <= 0f)
            {
                _currentState = GameState.Cooldown;
                _cooldownTimer = currentDifficulty.GetRandomWaveInterval();
                AdjustCooldownTimer();
                StopCoroutine(ActivateModulesWave());
            }
        }

        private void HandleCooldown(DifficultyParameters currentDifficulty)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f)
            {
                _currentState = GameState.Wave;
                _waveTimer = currentDifficulty.GetRandomWaveDuration();
                StartCoroutine(ActivateModulesWave());
                ResetCooldownModifiers();
            }
        }

        private void AdjustCooldownTimer()
        {
            if (!_cooldownAdjusted)
            {
                if (_halfCooldown)
                {
                    _cooldownTimer /= 2;
                }
                if (_noCooldown)
                {
                    _cooldownTimer = 0;
                }
                _cooldownAdjusted = true;
            }
        }

        private IEnumerator ActivateModulesWave()
        {
            var currentDifficulty = difficulties[_activeDifficulty];
            if (currentDifficulty == null)
            {
                yield break;
            }

            while (_currentState == GameState.Wave)
            {
                yield return new WaitForSeconds(Random.Range(3f, 5f));

                FindBreachModules();
                CountActiveBreach();
                ActivateRandomModule();
            }
        }

        public void FindBreachModules()
        {
            modules.Clear();

            BreachModule[] foundModules = FindObjectsOfType<BreachModule>();
            foreach (BreachModule module in foundModules)
            {
                modules.Add(module);
            }
        }

        public void CountActiveBreach()
        {
            activeModules = 0;
            foreach (BreachModule module in modules)
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
                foreach (SubmarinModule module in modules)
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

        public void HalfWaveCooldown()
        {
            _halfCooldown = true;
        }

        public void NoWaveCooldown()
        {
            _noCooldown = true;
        }

        public void ResetCooldownModifiers()
        {
            _halfCooldown = false;
            _noCooldown = false;
            _cooldownAdjusted = false;
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
                _waveTimer = selectedDifficulty.initialDelay;
                _activeDifficulty = phase1Value - 1;
                foreach (SubmarinModule module in modules)
                {
                    module.IsActivated = false;
                }
                _hasUpdatedDifficulty = true;
                _isDifficultySet = true;
                _currentState = GameState.InitialDelay;
                ResetCooldownModifiers(); // Reset cooldown modifiers when difficulty is updated
            }
            else
            {
                Debug.LogWarning("Invalid Phase1Value or Difficulty Parameters not set for this phase.");
            }
        }
    }
}
