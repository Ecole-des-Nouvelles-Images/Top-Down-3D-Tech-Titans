using System;
using Elias.Scripts.Managers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Christopher.Scripts
{
    public class PauseMenu : MonoBehaviourSingleton<PauseMenu>
    {
        public GameObject PausePanel;
        [SerializeField] private AudioMixer musiqueMixer;
        [SerializeField] private AudioMixer sfxMixer;
        [SerializeField] private AudioMixer ambianceMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider ambianceSlider;
        [SerializeField] private Button reprendreButton;

        // Variables to store the original volume levels
        private float originalSfxVolume;
        private float originalAmbianceVolume;

        private void Awake() {
            PausePanel.SetActive(false);
            
            sfxMixer.GetFloat("SFX", out originalSfxVolume);
            ambianceMixer.GetFloat("Ambiance", out originalAmbianceVolume);
        }

        public void TogglePauseMenuActivation() {
            if (PausePanel.activeSelf) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }

        private void PauseGame() {
            PausePanel.SetActive(true);
            reprendreButton.Select();
            sfxMixer.SetFloat("SFX", -80f);
            ambianceMixer.SetFloat("Ambiance", -80f);

            Time.timeScale = 0;
        }

        private void ResumeGame() {
            PausePanel.SetActive(false);

            sfxMixer.SetFloat("SFX", originalSfxVolume);
            ambianceMixer.SetFloat("Ambiance", originalAmbianceVolume);

            Time.timeScale = 1;
        }

        public void LeaveGame() {
            Application.Quit();
        }

        public void RebootGame() {
            // GameManager.Instance.GameOver("L'equipe est retournee à la vie civile");
        }

        public void SetMusicVolume() {
            float musicVolume = musicSlider.value;
            musiqueMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        }

        public void SetSFXVolume() {
            float sfxVolume = sfxSlider.value;
            sfxMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);

            // Update originalSfxVolume when the slider is adjusted
            sfxMixer.GetFloat("SFX", out originalSfxVolume);
        }

        private void SetAmbianceVolume() {
            float ambianceVolume = ambianceSlider.value;
            ambianceMixer.SetFloat("Ambiance", Mathf.Log10(ambianceVolume) * 20);

            // Update originalAmbianceVolume when the slider is adjusted
            ambianceMixer.GetFloat("Ambiance", out originalAmbianceVolume);
        }
    }
}
