using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEntity : MonoBehaviour
{
    public float ProgressTime;
    public int Damage;
    public int MaxEndurance;
    [SerializeField] private int moveSpeed;
    private float _currentTime;
    private int _currentEndurance;
    private int _currentMoveSpeed;
    private GameObject _drillArm;
    private Rigidbody _drillRB;
    // Start is called before the first frame update
    void Start()
    {
        _drillArm = gameObject.transform.parent.gameObject;
        _drillRB = _drillArm.gameObject.transform.GetComponent<Rigidbody>();
        _currentTime = ProgressTime;
        _currentEndurance = MaxEndurance;
        _currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveX(float moveX)
    {
        float xMov = moveX * -1;
        Vector3 velocity = transform.TransformDirection(new Vector3(xMov, 0, 0).normalized) * (_currentMoveSpeed * Time.fixedDeltaTime);
        _drillRB.velocity = new Vector3(velocity.x, _drillRB.velocity.y, velocity.z);
    }
    public void MoveY(float moveY)
    {
        float yMov = moveY;
        Vector3 velocity = transform.TransformDirection(new Vector3(0, yMov, 0).normalized) * (_currentMoveSpeed * Time.fixedDeltaTime);
        _drillRB.velocity = new Vector3(_drillRB.velocity.x, velocity.y, velocity.z);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Rock")) {
            if (_currentTime <= 0) {
                other.gameObject.GetComponent<RockEntity>().TakeDamage(Damage);
                _currentEndurance -= Damage;
                if (_currentEndurance < 0) _currentEndurance = 0;
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
