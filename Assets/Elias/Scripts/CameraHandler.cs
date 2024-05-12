using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.5f;
    public CinemachineTargetGroup targetGroup; // Référence au script CinemachineTargetGroup

    private Vector3 originalPosition;
    private float shakeTimer = 0f;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartShake();
        }

        if (shakeTimer > 0)
        {
            // Désactiver le script CinemachineTargetGroup
            if (targetGroup != null)
            {
                targetGroup.enabled = false;
            }

            Vector3 shakePosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            transform.position = shakePosition;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Activer à nouveau le script CinemachineTargetGroup une fois que le shake est terminé
            if (targetGroup != null)
            {
                targetGroup.enabled = true;
            }

            transform.position = originalPosition;
        }
    }

    public void StartShake()
    {
        shakeTimer = shakeDuration;
    }
}