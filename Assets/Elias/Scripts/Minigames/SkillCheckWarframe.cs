using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;

namespace Elias.Scripts.Minigames
{
    public class SkillCheckWarframe : SubmarinModule
    {
        public RectTransform indicatorNeedle;
        public List<RectTransform> successZones;
        public float rotationSpeed = 200f;
        private bool _isClockwise = true;
        public GameObject canvas;

        // Timing and pattern recognition
        public float timeLimit = 5f;
        private float timeRemaining;
        private int currentPatternIndex = 0;
        private bool isPatternMatched = false;

        void Update()
        {
            if (IsActivated)
            {
                RotateNeedle();
                CheckForInput();
                canvas.SetActive(true);
                UpdateTimer();
            }
            else
            {
                canvas.SetActive(false);
            }
        }

        void RotateNeedle()
        {
            float angle = rotationSpeed * Time.deltaTime;
            if (!_isClockwise) angle = -angle;
            indicatorNeedle.Rotate(0, 0, angle);

            if (indicatorNeedle.localEulerAngles.z >= 360f || indicatorNeedle.localEulerAngles.z <= 0f)
            {
                _isClockwise = !_isClockwise;
            }
        }

        void CheckForInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float needleAngle = indicatorNeedle.localEulerAngles.z;
                foreach (var zone in successZones)
                {
                    float successStartAngle = zone.localEulerAngles.z - (zone.rect.width / 2);
                    float successEndAngle = zone.localEulerAngles.z + (zone.rect.width / 2);

                    if (needleAngle > 180f) needleAngle -= 360f;
                    if (successStartAngle > 180f) successStartAngle -= 360f;
                    if (successEndAngle > 180f) successEndAngle -= 360f;

                    if (needleAngle >= successStartAngle && needleAngle <= successEndAngle)
                    {
                        isPatternMatched = true;
                        currentPatternIndex++;
                        if (currentPatternIndex >= successZones.Count)
                        {
                            Debug.Log("Skill check succeeded!");
                            Succes.Add(true);
                            if (States.Count > 0)
                            {
                                foreach (var obj in StateDisplayObject)
                                {
                                    obj.GetComponent<Renderer>().material = States[1];
                                }
                            }
                            IsActivated = false;
                        }
                        return;
                    }
                }

                Debug.Log("Skill check failed!");
                Succes.Add(false);
                if (States.Count > 0)
                {
                    foreach (var obj in StateDisplayObject)
                    {
                        obj.GetComponent<Renderer>().material = States[0];
                    }
                }
                IsActivated = false;
            }
        }

        void UpdateTimer()
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                Debug.Log("Time's up! Skill check failed!");
                Succes.Add(false);
                if (States.Count > 0)
                {
                    foreach (var obj in StateDisplayObject)
                    {
                        obj.GetComponent<Renderer>().material = States[0];
                    }
                }
                IsActivated = false;
            }
        }

        public override void Activate()
        {
            IsActivated = true;
            SetRandomSuccessZoneAngles();
            timeRemaining = timeLimit;
            currentPatternIndex = 0;
            isPatternMatched = false;
        }

        public override void Deactivate()
        {
            IsActivated = false;
        }

        public override void Interact(GameObject playerUsingModule)
        {
            indicatorNeedle.localEulerAngles = Vector3.zero;
            PlayerUsingModule = playerUsingModule;
            Activate();
        }

        public override void StopInteract()
        {
            PlayerUsingModule = null;
        }

        public override void Validate()
        {
        }

        public override void NavigateX(float moveX)
        {
        }

        public override void NavigateY(float moveY)
        {
        }

        private void SetRandomSuccessZoneAngles()
        {
            foreach (var zone in successZones)
            {
                float randomAngle = UnityEngine.Random.Range(30f, 330f);
                zone.localEulerAngles = new Vector3(zone.localEulerAngles.x, zone.localEulerAngles.y, randomAngle);
            }
        }
    }
}
