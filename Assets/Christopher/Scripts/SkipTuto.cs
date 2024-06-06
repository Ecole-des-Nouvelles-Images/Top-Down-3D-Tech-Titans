using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SkipTuto : MonoBehaviour {
    private void OnAction() { SceneManager.LoadScene("Integration"); }
}
