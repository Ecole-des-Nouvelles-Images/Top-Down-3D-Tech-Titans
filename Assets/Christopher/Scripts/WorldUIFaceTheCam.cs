using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIFaceTheCam : MonoBehaviour {
    private void Awake() {
        transform.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
    }
}
