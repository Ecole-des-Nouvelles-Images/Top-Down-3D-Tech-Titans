using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEntity : MonoBehaviour
{
    public int MaxHP;
    private int _currentHP;
    
    // Start is called before the first frame update
    void Start() {
        _currentHP = MaxHP;
    }

    public void TakeDamage(int damage) {
        _currentHP -= damage;
        if (_currentHP <= 0) {
            gameObject.SetActive(false);
        }
    }
}
