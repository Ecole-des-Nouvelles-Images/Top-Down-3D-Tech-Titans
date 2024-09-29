using System;
using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using Christopher.Scripts.Modules;
using Elias.Scripts.Camera;
using Elias.Scripts.Light;
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

        private CameraHandler _cameraHandler;

        public List<BreachModule> modules = new List<BreachModule>();
        public List<DifficultyParameters> difficulties;
        private bool _isDifficultySet;
        private bool _hasUpdatedDifficulty;
        private int _activeDifficulty;
        public bool noActiveBreach;

        private float _torpedoTimer;
        public float generatorTimer;
        private float _waveTimer;
        private float _cooldownTimer;
        private bool _halfCooldown;
        private bool _noCooldown;
        private bool _cooldownAdjusted;
        
        public int activePhase;
        private int _generatorEventCount = 0;

        private enum GameState { InitialDelay, Wave, Cooldown }
        private GameState _currentState;

        private AudioSource _audioSource;
        public AudioClip[] audioClips;
        
        private LightColorAnimation[] _lights;

        //private bool _gameReset;
        
        private void Awake()
        {
            _lights = FindObjectsOfType<LightColorAnimation>(true); // 'true' if you want inactive lights;
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _cameraHandler = FindObjectOfType<CameraHandler>();

            if (difficulties == null)
            {
                difficulties = new List<DifficultyParameters>();
            }

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
                generatorTimer = difficulties[_activeDifficulty].GetRandomGeneratorInterval();
                _torpedoTimer = difficulties[_activeDifficulty].GetRandomTorpedoInterval();
            }
            else
            {
                Debug.LogError("No difficulties set.");
            }
        }

        /*private void ResetAll()
        {
            // Reset active modules
            foreach (BreachModule module in modules)
            {
                module.Deactivate(); // Deactivate all breach modules
            }

            // Reset any other modules you have
            var generator = FindObjectOfType<GeneratorModule>();
            if (generator != null)
            {
                generator.Deactivate(); // Reset generator module
            }

            var torpedoLauncher = FindObjectOfType<TorpedoLauncherModule>();
            if (torpedoLauncher != null)
            {
                torpedoLauncher.Deactivate(); // Reset torpedo launcher module
            }

            // Reset timers
            _generatorTimer = difficulties[_activeDifficulty].GetRandomGeneratorInterval();
            _torpedoTimer = difficulties[_activeDifficulty].GetRandomTorpedoInterval();
            _waveTimer = difficulties[_activeDifficulty].initialDelay;

            // Reset cooldown states
            ResetCooldownModifiers();
    
            // Reset difficulty state
            _hasUpdatedDifficulty = false;
            _isDifficultySet = false;

            // Optionally reset the current state
            _currentState = GameState.InitialDelay; // Reset to the initial game state
            
            GameManager.Instance.LowerWaterToInitialPosition();
        }*/

        private void Update()
        {
            /*if (activePhase == 1 && !_gameReset)
            {
                ResetAll();
                _gameReset = true;
            }*/
            
            if (activePhase == 1)
            {
                return; 
            }
            /*else
            {
                _gameReset = false;
            }*/


            if (_currentState == GameState.Wave)
                StartCoroutine(AnimateColorCoroutine());

            if (generatorTimer >= 0)
            {
                generatorTimer -= Time.deltaTime;
            }

            if (_torpedoTimer >= 0)
            {
                _torpedoTimer -= Time.deltaTime;
            }

            // Generator logic
            if (generatorTimer <= 0 && (_generatorEventCount < difficulties[_activeDifficulty].generatorCountLimit))
            {
                GeneratorModule generator = FindObjectOfType<GeneratorModule>();
                if (generator != null)
                {
                    generator.Activate();
                    _generatorEventCount++;
                }
            }

            // Torpedo logic
            if (_torpedoTimer <= 0)
            {
                TorpedoLauncherModule torpedo = FindObjectOfType<TorpedoLauncherModule>();
                if (torpedo != null)
                {
                    torpedo.Activate();
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
        

            
            if (!_hasUpdatedDifficulty || !_isDifficultySet)
            {
                return;
            }

            currentDifficulty = difficulties[_activeDifficulty];
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

        public void ResetGeneratorTimer()
        {
            generatorTimer = difficulties[_activeDifficulty].GetRandomGeneratorInterval();
        }
        
        public void ResetTorpedoTimer()
        {
            _torpedoTimer = difficulties[_activeDifficulty].GetRandomTorpedoInterval();
        }

        private IEnumerator AnimateColorCoroutine()
        {
            float t = 0f;
            float duration = 0.25f;

            while (_currentState == GameState.Wave) // Make the lights red
            {
                if (t > 1) yield return null; // Keep them red while the state is still 'Wave'

                t += Time.deltaTime / duration;
                foreach (LightColorAnimation light in _lights)
                {
                    Color start = light.OriginalColor;
                    light.Light.color = Color.Lerp(start, Color.red, t);
                    yield return null;
                }
            }

            t = 0f; // Reset animation (Lerp)

            while (t < 1) // Return to normal color
            {
                t += Time.deltaTime / duration;
                foreach (LightColorAnimation light in _lights)
                {
                    Color start = light.CurrentColor;
                    light.Light.color = Color.Lerp(start, light.OriginalColor, t);
                    yield return null;
                }
            }
        }

        private void HandleInitialDelay(DifficultyParameters currentDifficulty)
        {
            _waveTimer -= Time.deltaTime;
            if (_waveTimer <= 0f)
            {
                PlayRandomAudioClip();
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
                StopCoroutine(ActivateModulesWave()); // Stop the wave coroutine
                StopCoroutine(AnimateColorCoroutine()); // Stop the color animation coroutine
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
                PlayRandomAudioClip();
            }
        }

        private void PlayRandomAudioClip()
        {
            if (audioClips != null && audioClips.Length > 0)
            {
                int randomClipIndex = Random.Range(0, audioClips.Length);
                _audioSource.clip = audioClips[randomClipIndex];
                _audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Audio clips array is empty or null.");
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

            _cameraHandler.StartShake();
            GameManager.Instance.Ragdoll();

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
                    randomModule.Activate();
                }
            }
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

        public void ActivateBreaches()
        {
            FindBreachModules();
            CountActiveBreach();
            ActivateRandomModule();
            ActivateRandomModule();
            difficulties[_activeDifficulty].GetRandomTorpedoInterval();
        }

    }
}
