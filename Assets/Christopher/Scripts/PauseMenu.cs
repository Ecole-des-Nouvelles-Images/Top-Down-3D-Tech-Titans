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
        private void Awake() {
            PausePanel.SetActive(false);
        }

        public void TogglePauseMenuActivation() {
            if (PausePanel.activeSelf) {
                PausePanel.SetActive(false); 
                //AudioListener.pause = false;
                Time.timeScale = 1;
            }
            else {
                PausePanel.SetActive(true);
                reprendreButton.Select();
                //AudioListener.pause = true;
                Time.timeScale = 0;
            }
        }
        public void LeaveGame() {
            Application.Quit();
        }
        public void RebootGame() {
            SceneManager.LoadScene("Integration");
        }
        public void SetMusicVolume() {
            float musicVolume = musicSlider.value;
            musiqueMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        }

        public void SetSFXVolume() {
            float sfxVolume = sfxSlider.value;
            sfxMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        }

        private void SetAmbianceVolume() {
            float ambianceVolume = ambianceSlider.value;
            ambianceMixer.SetFloat("Ambiance", Mathf.Log10(ambianceVolume) * 20);
        }
    }
}
