using System;
using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public int MoveSpeed;
    public float TimerToTakeDamage;
    [SerializeField] private GameObject screenModule;

    private bool _isRecovering;
    private Rigidbody2D _rB2Dsubmarine;
    private int _currentSpeed;
    private float _currentTimerToTakeDamage;
    private bool _leftMapLimit;
    private bool _rightMapLimit;
    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = MoveSpeed;
        _rB2Dsubmarine = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (_currentTimerToTakeDamage != 0) {
            _currentTimerToTakeDamage -= Time.deltaTime;
            transform.GetComponent<SpriteRenderer>().enabled = !transform.GetComponent<SpriteRenderer>().enabled;
            if (_currentTimerToTakeDamage < 0) _currentTimerToTakeDamage = 0;
        }

        if (_currentTimerToTakeDamage == 0) {
            _isRecovering = false;
            transform.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    
    public void MoveX(float moveX) {
        float xMov = moveX * -1;
        if (_leftMapLimit && xMov > 0)xMov = 0;
        if (_rightMapLimit && xMov < 0)xMov = 0;
        Vector2 velocity = transform.TransformDirection(new Vector2(xMov, 0).normalized) * (_currentSpeed * Time.fixedDeltaTime);
        _rB2Dsubmarine.velocity = new Vector3(velocity.x, _rB2Dsubmarine.velocity.y); 
    }

    public void MoveY(float moveY) {
        if (moveY >= 0) {
            float yMov = moveY;
            Vector2 velocity = transform.TransformDirection(new Vector2(0, yMov).normalized) *
                               (_currentSpeed * Time.fixedDeltaTime);
            _rB2Dsubmarine.velocity = new Vector2(_rB2Dsubmarine.velocity.x, velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isRecovering && !other.CompareTag("End") || !other.CompareTag("LeftMapLimit") || !other.CompareTag("RightMapLimit") ) {
            Debug.Log("obstacle touché !");
            screenModule.transform.GetComponent<ScreenSubmarinModule>().Succes.Add(false);
            _isRecovering = true;
            _currentTimerToTakeDamage = TimerToTakeDamage;
        }

        if (other.CompareTag("End"))
        {
            Debug.Log("partie fini");
            _rB2Dsubmarine.velocity = new Vector2(0, 0);
        }
        screenModule.transform.GetComponent<ScreenSubmarinModule>().Succes.Add(true);
        if (other.CompareTag("LeftMapLimit")) {
            _leftMapLimit = true;
        }
        if (other.CompareTag("RightMapLimit")) {
            _rightMapLimit = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other){
        if (!_isRecovering && !other.CompareTag("End")) {
            Debug.Log("obstacle touché !");
            screenModule.transform.GetComponent<ScreenSubmarinModule>().Succes.Add(false);
            _isRecovering = true;
            _currentTimerToTakeDamage = TimerToTakeDamage;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("LeftMapLimit")) {
            _leftMapLimit = false;
        }
        if (other.CompareTag("RightMapLimit")) {
            _rightMapLimit = false;
        }
    }
}
