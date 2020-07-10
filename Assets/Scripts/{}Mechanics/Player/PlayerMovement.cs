using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    private Vector2 _movementInput;
    private bool _jump = false;
    private bool _canMove = true;
    private Damageable _damagable;
    private void Start()
    {
        _damagable = GetComponent<Damageable>();
        _damagable.OnHit.AddListener(new UnityAction(() => _canMove = false));
        _damagable.OnHitRecovery.AddListener(new UnityAction(() => _canMove = true));
    }
    void FixedUpdate()
    {
        if (_canMove)
        {
            controller.Move(
                _movementInput.x * Time.fixedDeltaTime, 
                _movementInput.y * Time.fixedDeltaTime
            );
        }
    }

    public void OnMove(InputValue value)
    {
        var input = value.Get<Vector2>();
        _movementInput = input;
    }
    public void OnJump(InputValue value)
    {
        var input = value.Get<float>();
        _jump = input == 1f;

        if (_canMove)
        {
            controller.Jump(_jump);
        }
    }
    public void OnDash()
    {
        if (_canMove)
        {
            controller.Dash();
        }
    }
}
