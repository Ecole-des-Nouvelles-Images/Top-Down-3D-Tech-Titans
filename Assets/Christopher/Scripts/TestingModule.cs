using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Christopher.Scripts;
using UnityEngine;

public class TestingModule : MonoBehaviour
{
    public SubmarinModule ModuleToTest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");
        ModuleToTest.NavigateX(xMov);
        ModuleToTest.NavigateY(zMov);
        if (Input.GetButtonDown("Fire1")) {
            ModuleToTest.Validate();
        }
    }
}
