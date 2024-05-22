using Christopher.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Elias.Scripts.Minigames
{
    public class SkillCheckDbd : SubmarinModule
    {
        public RectTransform indicatorNeedle;
        public RectTransform successZone;
        public float rotationSpeed = 200f;
        private bool _isClockwise = true;
        public bool playerInteracting;
        public GameObject canvas;

        void Update()
        {
            if (playerInteracting)
            {
                RotateNeedle();
                CheckForInput();
                canvas.SetActive(true);
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
                float needleAngle = NormalizeAngle(indicatorNeedle.localEulerAngles.z);
                float successStartAngle = NormalizeAngle(successZone.localEulerAngles.z - (successZone.rect.width / 2));
                float successEndAngle = NormalizeAngle(successZone.localEulerAngles.z + (successZone.rect.width / 2));

                if (needleAngle >= successStartAngle && needleAngle <= successEndAngle)
                {
                    OnSkillCheckSuccess();
                }
                else
                {
                    OnSkillCheckFailure();
                }
            }
        }

        float NormalizeAngle(float angle)
        {
            if (angle > 180f) angle -= 360f;
            return angle;
        }

        void OnSkillCheckSuccess()
        {
            Debug.Log("Skill check succeeded!");
            Succes.Add(true);
            if (StatesMaterials.Length > 1)
            {
                UpdateStateDisplayObjects(StatesMaterials[1]);
            }
            playerInteracting = false; // Ensure playerInteracting is set to false on success
            canvas.SetActive(false); // Deactivate canvas after success
            Deactivate();
        }

        void OnSkillCheckFailure()
        {
            Debug.Log("Skill check failed!");
            Succes.Add(false);
            if (StatesMaterials.Length > 0)
            {
                UpdateStateDisplayObjects(StatesMaterials[0]);
            }
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
            State = 1;
            SetRandomSuccessZoneAngle();
        }

        public override void Deactivate()
        {
            IsActivated = false;
            State = 0;
        }

        public override void Interact(GameObject playerUsingModule)
        {
            playerInteracting = true;
            indicatorNeedle.localEulerAngles = Vector3.zero;
            PlayerUsingModule = playerUsingModule;
            Activate();
        }

        public override void StopInteract()
        {
            playerInteracting = false;
            PlayerUsingModule = null;
        }

        public override void Validate() { }

        public override void NavigateX(float moveX) { }

        public override void NavigateY(float moveY) { }

        public override void Up() { }

        public override void Down() { }

        public override void Left() { }

        public override void Right() { }

        private void SetRandomSuccessZoneAngle()
        {
            float randomAngle = UnityEngine.Random.Range(30f, 330f);
            successZone.localEulerAngles = new Vector3(successZone.localEulerAngles.x, successZone.localEulerAngles.y, randomAngle);
        }
    }
}
