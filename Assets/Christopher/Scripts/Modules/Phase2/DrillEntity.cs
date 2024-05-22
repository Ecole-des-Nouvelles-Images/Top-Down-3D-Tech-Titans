using System;
using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts.Modules;
using UnityEngine;
using Random = UnityEngine.Random;

public class DrillEntity : MonoBehaviour
{
    public float ProgressTime;
    public int Damage;
    public int MaxEndurance;
    public bool IsDamaged;
    public int fixValue;
    
    [SerializeField] private RectTransform fixingProgressBar;
    [SerializeField] private GameObject drillFixingModule;
    [SerializeField] private GameObject digEffect;
    [SerializeField] private int moveSpeed;
    
    private float _currentTime;
    private int _currentEndurance;
    private int _currentMoveSpeed;
    private GameObject _drillArm;
    private GameObject _currentDiggingRock;
    private Rigidbody _drillRB;
    private GameObject _playerUsingModule;
    
    public void MoveX(float moveX)
    {
        float xMov = moveX * -1;
        Vector3 velocity = transform.TransformDirection(new Vector3(xMov, 0, 0).normalized) * (_currentMoveSpeed * Time.fixedDeltaTime);
        _drillRB.velocity = new Vector3(velocity.x, _drillRB.velocity.y, _drillRB.velocity.z);
    }
    public void MoveY(float moveY)
    {
        float yMov = moveY * -1;
        Vector3 velocity = transform.TransformDirection(new Vector3(yMov, 0, 0).normalized) * (_currentMoveSpeed * Time.fixedDeltaTime);
        _drillRB.velocity = new Vector3(_drillRB.velocity.x,_drillRB.velocity.y , velocity.x);
    }

    public void FixDrill() {
        _currentEndurance += fixValue;
        if (_currentEndurance > MaxEndurance) _currentEndurance = MaxEndurance;
    }
    // Start is called before the first frame update
    private void Start()
    {
        _currentDiggingRock = null;
        _drillArm = gameObject.transform.parent.gameObject;
        _drillRB = _drillArm.gameObject.transform.GetComponent<Rigidbody>();
        _currentTime = ProgressTime;
        _currentEndurance = MaxEndurance;
        _currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        Helper.PourcentStateBarre(fixingProgressBar,'x',_currentEndurance,MaxEndurance);
        if (_currentEndurance == MaxEndurance) IsDamaged = false;
        if (_currentDiggingRock != null && !_currentDiggingRock.activeSelf) _currentDiggingRock = null;
        if(_currentDiggingRock != null)digEffect.SetActive(true);
        else
        {
            digEffect.SetActive(false);
        }
        if (IsDamaged) {
            transform.GetComponent<Collider>().enabled = false;
            drillFixingModule.transform.GetComponent<FixingDrillModule>().IsActivated = true;
        }
        else
        {
            transform.GetComponent<Collider>().enabled = true;
            drillFixingModule.transform.GetComponent<FixingDrillModule>().IsActivated = false;
        }
    }

    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Rock"))
        {
            _currentDiggingRock = other.gameObject;
            if (_currentTime <= 0) {
                other.gameObject.GetComponent<RockEntity>().TakeDamage(Damage);
                _currentEndurance -= Random.Range(0,Damage);
                if (_currentEndurance < 0) {
                    _currentEndurance = 0;
                    IsDamaged = true;
                }
                _currentTime = ProgressTime;
            }
            else {
                
                _currentTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Rock") ) {
            if (_currentDiggingRock == null) _currentDiggingRock = other.gameObject;
            if(other.gameObject.activeSelf)digEffect.SetActive(true);
            if (_currentTime <= 0) {
                other.gameObject.GetComponent<RockEntity>().TakeDamage(Damage);
                _currentEndurance -= Damage;
                if (_currentEndurance < 0) {
                    _currentEndurance = 0;
                    IsDamaged = true;
                }
                _currentTime = ProgressTime;
            }
            else {
                
                _currentTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            _currentDiggingRock = null;
        }
    }
}
