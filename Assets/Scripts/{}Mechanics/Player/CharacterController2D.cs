using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField]
    private float _walkSpeed = 400f;
    [Range(0, .3f)]
    [SerializeField]
    [Tooltip("How much to smooth out the movement")]
    private float _walkDampingBasic = .05f;
    [Range(0, .3f)]
    [SerializeField]
    [Tooltip("How much to smooth out the movement when stopping")]
    private float _walkDampingWhenStopping = .08f;
    [Range(0, .3f)]
    [SerializeField]
    [Tooltip("How much to smooth out the movement when turning")]
    private float _walkDampingWhenTurning = .06f;

    // For determining which way the player is currently facing.
    private bool _facingRight = true;

    [Header("Jump")]
    [SerializeField]
    [Tooltip("Amount of force added when the player jumps")]
    private float _jumpForce = 40f;
    [SerializeField]
    [Tooltip("Whether or not a player can steer while jumping")]
    private bool _airControl = false;
    [SerializeField]
    [Tooltip("Seconds to buffer a jump action when landing")]
    private float _jumpBufferWhenLanding = 0.2f;
    [SerializeField]
    [Tooltip("Seconds to buffer a jump action when falling")]
    private float _jumpBufferWhenFalling = 0.2f;
    [SerializeField]
    [Tooltip("Seconds to buffer a jump action when climbing")]
    private float _jumpBufferWhenClimbing = 0.2f;
    [SerializeField]
    float fallMultiplier = 2.5f;
    [SerializeField]
    float lowJumpMultiplier = 2f;

    // To store the pressed state of the jump button
    private bool _jumpPressed;
    // The time when the player pressed jump for the last time
    private float _timeLastJumpPressed;


    [Header("Dash")]
    [SerializeField]
    [Tooltip("Amount of horizontal force added when the player dashes")]
    private float _dashForce = 20f;
    [SerializeField]
    [Tooltip("Amount of time when the player's movement is only affected by the dash")]
    private float _dashDuration = 0.2f;
    [SerializeField]
    [Tooltip("Cooldown between consecutive dashes")]
    private float _dashCooldown = 0.5f;

    private bool _canDash = true;
    private bool _isDashing;

    [Header("Collision Check")]
    [SerializeField]
    [Tooltip("A mask determining what is ground to the character")]
    private LayerMask _groundLayer;
    [SerializeField]
    [Tooltip("A position marking where to check if the player is grounded")]
    private Vector3 _groundCheckOffset;

    // The time when the player was grounded for the last time
    private float _timeLastGrounded;
    // Radius of the overlap circle to determine if grounded
    private const float _groundedRadius = .2f;
    // Whether or not the player is grounded.
    private bool _grounded;

    [Header("Climb")]
    [SerializeField]
    private float _climbLateralSpeed = 300f;
    [SerializeField]
    private float _climbUpSpeed = 200f;
    [SerializeField]
    private float _climbDownSpeed = 300f;
    [Range(0, .3f)]
    [SerializeField]
    [Tooltip("How much to smooth out the climbing movement")]
    private float _climbDamping = .05f;
    [SerializeField]
    [Tooltip("A mask determining what is climbable to the character")]
    private LayerMask _climbLayer;

    private bool _canClimb;
    private bool _isClimbing;
    // The time when the player was climbing for the last time
    private float _timeLastClimbing;

    // Physics variables
    private Rigidbody2D _rigidbody2D;
    private Vector3 _velocity = Vector3.zero;
    private float _gravityScale;

    [Header("Events")]
    
    public UnityEvent OnFlipEvent;
    public UnityEvent OnJumpEvent;
    public UnityEvent OnLandEvent;
    public UnityEvent OnDashAvailableEvent;
    public UnityEvent OnDashStartEvent;
    public UnityEvent OnDashEndEvent;
    public UnityEvent OnClimbStartEvent;
    public UnityEvent OnClimbEndEvent;
    #endregion

    #region Unity callbacks
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gravityScale = _rigidbody2D.gravityScale;

        if (OnFlipEvent == null)
            OnFlipEvent = new UnityEvent();
        if (OnJumpEvent == null)
            OnJumpEvent = new UnityEvent();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnDashAvailableEvent == null)
            OnDashAvailableEvent = new UnityEvent();
        if (OnDashStartEvent == null)
            OnDashStartEvent = new UnityEvent();
        if (OnDashEndEvent == null)
            OnDashEndEvent = new UnityEvent();
        if (OnClimbStartEvent == null)
            OnClimbStartEvent = new UnityEvent();
        if (OnClimbEndEvent == null)
            OnClimbEndEvent = new UnityEvent();
    }
    private void FixedUpdate()
    {
        UpdateGroundedState();
        UpdateJumpState();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsClimbLayer(collision.gameObject.layer))
        {
            _canClimb = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsClimbLayer(collision.gameObject.layer))
        {
            _canClimb = false;
            if (_isClimbing)
            {
                CancelClimb();
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Show the overlap circle to determine if grounded
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _groundCheckOffset, _groundedRadius);
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Moves the player according to the configured movement parameters.
    /// </summary>
    /// <param name="horizontalMove">A float value that indicates the horizontal movement of the player</param>
    public void Move(float horizontalMove, float verticalMove)
    {
        HandleWalk(horizontalMove);
        HandleClimb(horizontalMove, verticalMove);
        HandleFlip(horizontalMove);
    }

    /// <summary>
    /// Sets the jump button's pressed state. Triggers a jump according to the configured jump parameters 
    /// when the jump button is pressed if the player is in grounded state or the button was pressed 
    /// within a specified time window since the player left the ground.
    /// </summary>
    /// <param name="isJumpPressed">The button state. True when pressed</param>
    public void Jump(bool isJumpPressed)
    {
        _jumpPressed = isJumpPressed;
        if (_jumpPressed)
        {
            _timeLastJumpPressed = Time.time;
            if (_isClimbing || IsBufferValid(_timeLastClimbing, _jumpBufferWhenClimbing) ||
                _grounded || IsBufferValid(_timeLastGrounded, _jumpBufferWhenFalling))
            {
                CancelClimb();
                DoJump();
            }
        }
    }

    /// <summary>
    /// Triggers a horizontal dash to the configured dash parameters, using the current faced direction.
    /// </summary>
    public void Dash()
    {
        if (_canDash && !_isDashing)
        {
            DoDash();
        }
    }
    #endregion

    #region Collision check methods
    private void UpdateGroundedState()
    {
        bool wasGrounded = _grounded;
        _grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + _groundCheckOffset, _groundedRadius, _groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _grounded = true;
                _timeLastGrounded = Time.time;
                if (!wasGrounded)
                {
                    OnLanding();
                }
                break;
            }
        }
    }
    #endregion

    #region Movement methods
    private void HandleWalk(float move)
    {
        //only control the player if grounded or airControl is turned on
        if (!_isClimbing && !_isDashing && (_grounded || _airControl))
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * _walkSpeed, _rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            float movementSmoothing = _walkDampingBasic;
            if (Mathf.Abs(targetVelocity.x) < 0.1f)
            {
                movementSmoothing = _walkDampingWhenStopping;
            }
            else if (Mathf.Sign(targetVelocity.x) != Mathf.Sign(_rigidbody2D.velocity.x))
            {
                movementSmoothing = _walkDampingWhenTurning;
            }

            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, movementSmoothing);
        }
    }

    private void HandleFlip(float move)
    {
        // If the input is moving the player right and the player is facing left...
        // Otherwise if the input is moving the player left and the player is facing right...
        if ((move > 0 && !_facingRight) || (move < 0 && _facingRight))
        {
            OnFlipEvent.Invoke();
            // Switch the way the player is labelled as facing.
            _facingRight = !_facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    #endregion

    #region Jump methods
    private void UpdateJumpState()
    {
        if (_rigidbody2D.gravityScale > 0)
        {
            if (_rigidbody2D.velocity.y < 0)
            {
                _rigidbody2D.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (_rigidbody2D.velocity.y > 0 && !_jumpPressed)
            {
                _rigidbody2D.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
    }
    private void OnLanding()
    {
        if (IsBufferValid(_timeLastJumpPressed, _jumpBufferWhenLanding))
        {
            DoJump();
        }
        else
        {
            OnLandEvent.Invoke();
        }
    }
    private void DoJump()
    {
        OnJumpEvent.Invoke();
        _grounded = false;
        _timeLastGrounded = 0;
        _timeLastJumpPressed = 0;
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
    }
    private void CancelGravity()
    {
        if (_rigidbody2D.gravityScale != 0f)
        {
            _gravityScale = _rigidbody2D.gravityScale;
        }
        _rigidbody2D.gravityScale = 0f;
    }
    private void RestoreGravity()
    {
        _rigidbody2D.gravityScale = _gravityScale;
    }
    private bool IsBufferValid(float savedTime, float buffer)
    {
        return (Time.time - savedTime) <= buffer;
    }
    #endregion

    #region Dash methods
    private void DoDash()
    {
        OnDashStartEvent.Invoke();
        _rigidbody2D.velocity = new Vector2(_dashForce * (_facingRight ? 1f : -1f), 0f);
        CancelGravity();
        StartCoroutine(DashCooldown());
    }
    IEnumerator DashCooldown()
    {
        _isDashing = true;
        _canDash = false;
        yield return new WaitForSeconds(_dashDuration);
        if (!_isClimbing)
        {
            RestoreGravity();
        }
        _isDashing = false;
        OnDashEndEvent.Invoke();
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
        OnDashAvailableEvent.Invoke();
    }
    #endregion

    #region Climb methods
    private void HandleClimb(float moveX, float moveY)
    {
        if (!_isClimbing && _canClimb && ((moveY > 0f) || (moveY < 0f)))
        {
            StartClimb();
        }
        if (_isClimbing && !_isDashing)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector3(moveX * _climbLateralSpeed,
                moveY * (moveY > 0 ? _climbUpSpeed : _climbDownSpeed), 0f);
            // And then smoothing it out and applying it to the character
            _rigidbody2D.velocity = Vector3.SmoothDamp(
                _rigidbody2D.velocity, targetVelocity, ref _velocity, _climbDamping);
        }
    }
    private bool IsClimbLayer(int layer)
    {
        return ((1 << layer) & _climbLayer) != 0;
    }
    private void StartClimb()
    {
        // Check if the character is close to the highest point of a jump
        if (_rigidbody2D.velocity.y < 1f)
        {
            OnClimbStartEvent.Invoke();
            _isClimbing = true;
            CancelGravity();
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void CancelClimb()
    {
        OnClimbEndEvent.Invoke();
        _isClimbing = false;
        _timeLastClimbing = Time.time;
        if (!_isDashing)
        {
            RestoreGravity();
        }
    }
    #endregion
}
