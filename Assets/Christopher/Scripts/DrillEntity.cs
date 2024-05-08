using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEntity : MonoBehaviour
{
    public float ProgressTime;
    public int Damage;
    public int MaxEndurance;
    private float _currentTime;
    private int _currentEndurance;
    // Start is called before the first frame update
    void Start()
    {
        _currentTime = ProgressTime;
        _currentEndurance = MaxEndurance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Rock")) {
            if (_currentTime <= 0) {
                other.gameObject.GetComponent<RockEntity>().TakeDamage(Damage);
                _currentEndurance -= Damage;
                _currentTime = ProgressTime;
            }
            else {
                _currentTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Rock")) {
            if (_currentTime <= 0) {
                other.gameObject.GetComponent<RockEntity>().TakeDamage(Damage);
                _currentEndurance -= Damage;
                _currentTime = ProgressTime;
            }
            else {
                _currentTime -= Time.deltaTime;
            }
        }
    }
}
