using System;
using System.Collections;
using Christopher.Scripts;
using Elias.Scripts.Managers;
using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Elias.Scripts.Minigames
{
    public class BreachModule : SubmarinModule
    {
        public RectTransform indicatorNeedle;
        public RectTransform successZone;
        public float rotationSpeed = 200f;
        private bool _isClockwise = true;
        public bool playerInteracting;
        public GameObject canvas;
        private bool _needleStopped;

        public GameObject skin;
        
        public GameObject aboveWater;
        public GameObject underWater;

        public AudioSource breachAudioSource;
        public AudioSource audioSource;
        public AudioClip[] audioClips;
        public AudioClip[] audioBreachClips;
        
        private bool isAudioPlaying = false;
        private static int lastClipIndex = -1;
        private static int lastBreachClipIndex = -1;

        private void Start()
        {
            skin.SetActive(false);
            playerDetector.SetActive(false);
        }

        void Update()
        {
            if (playerInteracting)
            {
                RotateNeedle();
                canvas.SetActive(true);
            }
            else
            {
                canvas.SetActive(false);
            }

            if (GameManager.Instance.waterWalk)
            {
                aboveWater.SetActive(false);
                underWater.SetActive(true);
            }
            else
            {
                aboveWater.SetActive(true);
                underWater.SetActive(false);
            }
        }

        void RotateNeedle()
        {
            if (!_isClockwise)
            {
                return; // Exit the method if the needle is stopped
            }

            float angle = rotationSpeed * Time.deltaTime;
            indicatorNeedle.Rotate(0, 0, angle);

            if (indicatorNeedle.localEulerAngles.z >= 360f || indicatorNeedle.localEulerAngles.z <= 0f)
            {
                _isClockwise = !_isClockwise;
            }
        }



        float NormalizeAngle(float angle)
        {
            return Mathf.Repeat(angle, 360);
        }

        void OnSkillCheckSuccess()
        {
            Debug.Log("Skill check succeeded!");
            Succes.Add(true);
            if (StatesMaterials.Length > 1)
            {
                UpdateStateDisplayObjects(StatesMaterials[1]);
            }
            StartCoroutine(DeactivateAfterDelay());

            _isClockwise = false;
            indicatorNeedle.localEulerAngles = new Vector3(indicatorNeedle.localEulerAngles.x, indicatorNeedle.localEulerAngles.y, NormalizeAngle(indicatorNeedle.localEulerAngles.z));
            _needleStopped = true;
        }



        void OnSkillCheckFailure()
        {
            Debug.Log("Skill check failed!");
            Succes.Add(false);
            if (StatesMaterials.Length > 0)
            {
                UpdateStateDisplayObjects(StatesMaterials[0]);
            }

            StartCoroutine(ResetNeedleAfterDelay());

            _isClockwise = false;
            indicatorNeedle.localEulerAngles = new Vector3(indicatorNeedle.localEulerAngles.x, indicatorNeedle.localEulerAngles.y, NormalizeAngle(indicatorNeedle.localEulerAngles.z));
            _needleStopped = true;
        }




        void UpdateStateDisplayObjects(Material material)
        {
            foreach (var obj in StateDisplayObject)
            {
                obj.GetComponent<Renderer>().material = material;
            }
        }

        public override void Activate()
        {
            IsActivated = true;
            skin.SetActive(true);
            playerDetector.SetActive(true);
            State = 1;
            SetRandomSuccessZoneAngle();

            if (!isAudioPlaying)
            {
                int randomIndex;
                do
                {
                    randomIndex = new System.Random(Guid.NewGuid().GetHashCode()).Next(0, audioBreachClips.Length);
                } while (randomIndex == lastBreachClipIndex);
                lastBreachClipIndex = randomIndex;
                breachAudioSource.clip = audioBreachClips[randomIndex];
                breachAudioSource.Play();
                isAudioPlaying = true;
                StartCoroutine(PlayRandomClip());
                
            }
        }

        public override void Deactivate()
        {
            IsActivated = false;
            skin.SetActive(false);
            playerDetector.SetActive(false);
            State = 0;
            canvas.SetActive(false);
            
            if (PlayerUsingModule != null)
            {
                var playerController = PlayerUsingModule.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.QuitInteraction();
                }
            }
            
            isAudioPlaying = false;
            audioSource.Stop();
        }

        public override void Interact(GameObject playerUsingModule)
        {
            Activate();
            if (IsActivated && PlayerUsingModule == null) {
                PlayerUsingModule = playerUsingModule;
                PlayerUsingModule.transform.GetComponent<PlayerController>().inputActivatePanel.SetActive(true);
                
            }
            playerInteracting = true;
            indicatorNeedle.localEulerAngles = Vector3.zero;

            // Add these lines to check if the needle is stopped and resume its rotation
            if (_needleStopped)
            {
                _isClockwise = true;
                _needleStopped = false;
            }
            
            float successStartAngle = NormalizeAngle(successZone.localEulerAngles.z - 15);
            float successEndAngle = NormalizeAngle(successZone.localEulerAngles.z + 15);
            Debug.LogFormat("Success start angle: {0}, success end angle: {1}", successStartAngle, successEndAngle);
            
        }

        public override void StopInteract()
        {
            PlayerUsingModule.transform.GetComponent<PlayerController>().inputActivatePanel.SetActive(false);
            PlayerUsingModule = null;
            playerInteracting = false;
        }

        public override void Validate()
        {
            float needleAngle = NormalizeAngle(indicatorNeedle.localEulerAngles.z);
            float successStartAngle = NormalizeAngle(successZone.localEulerAngles.z - 15);
            float successEndAngle = NormalizeAngle(successZone.localEulerAngles.z + 15);

            float tolerance = 5f;
            if (needleAngle >= successStartAngle - tolerance && needleAngle <= successEndAngle + tolerance)
            {
                OnSkillCheckSuccess();
            }
            else
            {
                OnSkillCheckFailure();
            }
        }

        public override void NavigateX(float moveX) { }

        public override void NavigateY(float moveY) { }

        public override void Up() { }

        public override void Down() { }

        public override void Left() { }

        public override void Right() { }

        private void SetRandomSuccessZoneAngle()
        {
            float randomAngle = Random.Range(30f, 330f);
            successZone.localEulerAngles = new Vector3(successZone.localEulerAngles.x, successZone.localEulerAngles.y, randomAngle);
        }

        private IEnumerator ResetNeedleAfterDelay()
        {
            yield return new WaitForSeconds(1f);
            _isClockwise = true;
            indicatorNeedle.localEulerAngles = Vector3.zero;
        }
        
        private IEnumerator DeactivateAfterDelay()
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            Deactivate();
        }
        
        private IEnumerator PlayRandomClip()
        {
            while (isAudioPlaying)
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, audioClips.Length);
                } while (randomIndex == lastClipIndex);

                lastClipIndex = randomIndex;
                audioSource.clip = audioClips[randomIndex];
                audioSource.Play();

                yield return new WaitForSeconds(audioSource.clip.length);
            }
        }
        
    }
}