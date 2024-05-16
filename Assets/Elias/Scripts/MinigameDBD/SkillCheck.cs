using Christopher.Scripts;
using UnityEngine;

namespace Elias.Scripts.MinigameDBD
{
    public class SkillCheck : SubmarinModule
    {
        public RectTransform indicatorNeedle;
        public RectTransform successZone;
        public float rotationSpeed = 200f;
        private bool _isClockwise = true;

        void Start()
        {
            SetRandomSuccessZoneAngle();
        }

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
            SetRandomSuccessZoneAngle();
        }

        public override void Deactivate()
        {
            IsActivated = false;
        }

        public override void Interact(GameObject playerUsingModule)
        {
            indicatorNeedle.localEulerAngles = Vector3.zero;
            PlayerUsingModule = playerUsingModule;
            //Activate();
        }

        public override void StopInteract()
        {
            throw new System.NotImplementedException();
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

        private void SetRandomSuccessZoneAngle()
        {
            float randomAngle = UnityEngine.Random.Range(30f, 330f);
            successZone.localEulerAngles = new Vector3(successZone.localEulerAngles.x, successZone.localEulerAngles.y, randomAngle);
        }
    }
}
