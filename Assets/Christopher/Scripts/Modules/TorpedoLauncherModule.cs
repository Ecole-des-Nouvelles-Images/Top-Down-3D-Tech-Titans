using Elias.Scripts.Managers;
using Elias.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Christopher.Scripts.Modules
{
    public class TorpedoLauncherModule : SubmarinModule
    {
        public Sprite[] SelectedFond;
        public TMP_Text[] DisplayToDo;
        public GameObject[] DisplayDoingInput;
        public TMP_Text[] DisplayDoingContent;
        public GameObject PartyGameDisplay;
        public Animator TorpedoLauncherAnimator;
        [SerializeField] private AudioClip[] sounds; // 0:open door  1:close door   2:shoot sound
        [SerializeField] private AudioSource doorAudioSource;
        [SerializeField] private AudioSource shootAudioSource;
        private int[] _toDo = new int[3];
        private int[] _doing = new int[3];
        private int _currentSlot;
        private float _activationTimer = 20f;

        void Start() {
            shootAudioSource.clip = sounds[2];
            TorpedoLauncherAnimator.SetBool("isDoorOpen", true);
            PartyGameDisplay.SetActive(false);
            PlayerUsingModule = null;
            IsActivated = false; // Start with IsActivated as false
            if (_toDo == null || _doing == null)
            {
                Debug.LogError("ToDo or Doing arrays are not initialized!");
                return;
            }
            ResetPartyGame();
        }

        void Update() {
            
            if (IsActivated) {
                _activationTimer -= Time.deltaTime;

                if (_activationTimer <= 0) {
                    FailMiniGame();
                }

                State = 1;
                playerDetector.SetActive(true);
                for (int i = 0; i < _toDo.Length; i++) {
                    if (i != _currentSlot) DisplayDoingInput[i].GetComponent<Image>().sprite = SelectedFond[0];
                    else { DisplayDoingInput[i].GetComponent<Image>().sprite = SelectedFond[1]; }
                }
                if (_toDo.Length > 0) {
                    for (int i = 0; i < _toDo.Length; i++) {
                        DisplayToDo[i].text = _toDo[i].ToString();
                        DisplayDoingContent[i].text = _doing[i].ToString();
                    }
                } else {
                    for (int i = 0; i < DisplayDoingInput.Length; i++) {
                        DisplayDoingInput[i].GetComponent<Image>().sprite = null;
                    }
                }
                if (Verif()) {
                    Succes.Add(true);
                    shootAudioSource.Play();
                    Deactivate();
                    GameCycleController.Instance.ResetTorpedoTimer();
                }
            } else {
                State = 0;
                playerDetector.SetActive(false);
            }

            Material[] mats = StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials;
            mats[3] = StatesMaterials[State];
            StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials = mats;
        }

        private void ResetPartyGame()
        {
            if (_toDo == null || _doing == null)
            {
                Debug.LogError("ToDo or Doing arrays are not initialized!");
                return;
            }

            for (int i = 0; i < _toDo.Length; i++)
            {
                _toDo[i] = Random.Range(0, 9);
                _doing[i] = Random.Range(0, 9);
            }
        }

        public override void Activate() {
                IsActivated = true;
                State = 1;
        }


        public override void Deactivate() {
            IsActivated = false; // Set IsActivated to false
            State = 0;
            PartyGameDisplay.SetActive(false);
            if (PlayerUsingModule) {
                PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                PlayerUsingModule = null;
            }
            TorpedoLauncherAnimator.SetBool("isDoorOpen", true);
            doorAudioSource.clip = sounds[0];
            doorAudioSource.Play();
        }

        public override void Interact(GameObject playerUsingModule) {
            if (IsActivated && PlayerUsingModule == null && playerUsingModule.GetComponent<PlayerController>().MyItem == 3) {
                PlayerUsingModule = playerUsingModule;
                PartyGameDisplay.SetActive(true);
                PlayerUsingModule.GetComponent<PlayerController>().MyItem = 0;
                TorpedoLauncherAnimator.SetBool("isDoorOpen", false);
                doorAudioSource.clip = sounds[1];
                doorAudioSource.Play();
            } else {
                playerUsingModule.GetComponent<PlayerController>().QuitInteraction();
            }
        }

        public override void StopInteract() {
            PlayerUsingModule = null;
            PartyGameDisplay.SetActive(false);
        }

        public override void Validate() {}

        public override void NavigateX(float moveX) {}

        public override void NavigateY(float moveY) {}

        public override void Up() {
            if (_doing[_currentSlot] == 9) _doing[_currentSlot] = 0;
            else _doing[_currentSlot]++;
        }

        public override void Down() {
            if (_doing[_currentSlot] == 0) _doing[_currentSlot] = 9;
            else _doing[_currentSlot]--;
        }

        public override void Left() {
            if (_currentSlot == 0) _currentSlot = 2;
            else _currentSlot--;
        }

        public override void Right() {
            if (_currentSlot == 2) _currentSlot = 0;
            else _currentSlot++;
        }

        private bool Verif() {
            for (int i = 0; i < _toDo.Length; i++) {
                if (_toDo[i] != _doing[i]) return false;
            }
            return true;
        }

        private void FailMiniGame() {
            Deactivate();
            GameCycleController.Instance.ResetTorpedoTimer();
            PartyGameDisplay.SetActive(false);
            if (PlayerUsingModule) {
                PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                PlayerUsingModule = null;
            }
            TorpedoLauncherAnimator.SetBool("isDoorOpen", false);
            OnMiniGameFailed();
        }

        private void OnMiniGameFailed() {
            GameCycleController.Instance.ActivateBreaches();
        }
    }
}
