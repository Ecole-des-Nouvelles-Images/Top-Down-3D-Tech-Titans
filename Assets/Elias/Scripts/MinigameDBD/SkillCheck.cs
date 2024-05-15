using Christopher.Scripts;
using UnityEngine;

namespace Elias.Scripts
{
    public class SkillCheck : SubmarinModule
    {
        public RectTransform indicatorNeedle;
        public RectTransform successZone;
        public float rotationSpeed = 200f;
        private bool isClockwise = true;

        void Update()
        {
            if (IsActivated)
            {
                RotateNeedle();
                CheckForInput();
            }
        }

        void RotateNeedle()
        {
            float angle = rotationSpeed * Time.deltaTime;
            if (!isClockwise) angle = -angle;
            indicatorNeedle.Rotate(0, 0, angle);

            if (indicatorNeedle.localEulerAngles.z >= 360f || indicatorNeedle.localEulerAngles.z <= 0f)
            {
                isClockwise = !isClockwise;
            }
        }

        void CheckForInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float needleAngle = indicatorNeedle.localEulerAngles.z;
                float successStartAngle = successZone.localEulerAngles.z - (successZone.rect.width / 2);
                float successEndAngle = successZone.localEulerAngles.z + (successZone.rect.width / 2);

                if (needleAngle >= successStartAngle && needleAngle <= successEndAngle)
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
                }
                else
                {
                    Debug.Log("Skill check failed!");
                    Succes.Add(false);
                    if (States.Count > 0)
                    {
                        foreach (var obj in StateDisplayObject)
                        {
                            obj.GetComponent<Renderer>().material = States[0];
                        }
                    }
                }
                IsActivated = false;
            }
        }

        public override void Activate()
        {
            IsActivated = true;
            // Reset needle position if necessary
            indicatorNeedle.localEulerAngles = Vector3.zero;
        }

        public override void Deactivate()
        {
            IsActivated = false;
        }

        public override void Interact(GameObject playerUsingModule)
        {
            _playerUsingModule = playerUsingModule;
            Activate();
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
    }
}
