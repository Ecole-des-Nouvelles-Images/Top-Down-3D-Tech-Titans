using System;
using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public int MoveSpeed;
    public float TimerToTakeDamage;
    [SerializeField] private GameObject screenModule;
    [SerializeField] private GameObject tilemap;
    private bool _isRecovering;
    private Rigidbody2D _rB2Dsubmarine;
    private int _currentSpeed;
    private float _currentTimerToTakeDamage;
    private bool _leftMapLimit;
    private bool _rightMapLimit;
    private Vector3 _originPosition;
    void Start()
    {
        _originPosition = transform.position;
        _currentSpeed = MoveSpeed;
        _rB2Dsubmarine = transform.GetComponent<Rigidbody2D>();
        _isRecovering = false;
        _currentTimerToTakeDamage = 0;
    }
    void Update() {
        if (_currentTimerToTakeDamage != 0) {
            _currentTimerToTakeDamage -= Time.deltaTime;
            transform.GetComponent<SpriteRenderer>().enabled = !transform.GetComponent<SpriteRenderer>().enabled;
            if (_currentTimerToTakeDamage < 0) _currentTimerToTakeDamage = 0;
        }

        if (_currentTimerToTakeDamage <= 0) {
            _isRecovering = false;
            transform.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void ResetPosition() {
        transform.position = _originPosition;
    }
    public void MoveX(float moveX) {
        float xMov = moveX * -1;
        if (_leftMapLimit && xMov > 0)xMov = 0;
        if (_rightMapLimit && xMov < 0)xMov = 0;
        Vector2 velocity = transform.TransformDirection(new Vector2(xMov, 0).normalized) * (_currentSpeed * Time.fixedDeltaTime);
        _rB2Dsubmarine.velocity = new Vector3(velocity.x, _rB2Dsubmarine.velocity.y); 
    }

    public void MoveY(float moveY) {
        if (moveY >= 0.3) {
            float yMov = moveY;
            Vector2 velocity = transform.TransformDirection(new Vector2(0, yMov).normalized) *
                               (_currentSpeed * Time.fixedDeltaTime);
            _rB2Dsubmarine.velocity = new Vector2(_rB2Dsubmarine.velocity.x, velocity.y);
        }
        if (moveY <= 0) {
            float yNegativeMov = -0.5f;
            Vector2 velocity = transform.TransformDirection(new Vector2(0, yNegativeMov).normalized) *
                               (_currentSpeed * Time.fixedDeltaTime);
            _rB2Dsubmarine.velocity = new Vector2(_rB2Dsubmarine.velocity.x, velocity.y);
        }
    }
    public void BoostOn() {
        _currentSpeed *= 2;
    }
    public void BoostOff() {
        _currentSpeed = MoveSpeed;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("obstacle touché !");
        if (!_isRecovering && other.gameObject.Equals(tilemap)) {
            Debug.Log("obstacle touché !");
            screenModule.transform.GetComponent<ScreenModule>().Succes.Add(false);
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
