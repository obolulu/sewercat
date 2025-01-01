using System;
using System.Collections;
using _Project._Scripts.PlayerScripts.Stats;
using _Project._Scripts.ScriptBases;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerController : StateManager<PlayerController.PlayerState>, IDamageable
    {
        public enum PlayerState
        {
            Idle,
            Walking,
            Crouching,
            Falling,
            Jumping,
            Locked,
            Focused,
            Leaping
        }
        
        public BaseState<PlayerState> currentPlayerState => CurrentState; 
        
        [TitleGroup("Movement Settings")]
        [Header("Movement Settings")]
        public float moveSpeed = 6f;
        public  float   acceleration  = 20f; // New: Acceleration value
        public  float   deceleration  = 30f;
        public  float   rotationSpeed = 15f;
        private Vector3 _targetMoveVelocity;
        private Vector3 _currentMoveVelocity;
        private float   _targetSpeedMultiplier;

        [Header("Crouch Settings")] 
        [SerializeField] private float crouchHeight = 0.5f;
        [SerializeField] private float crouchSpeedMultiplier = 0.6f;
        private float _standingHeight;


        [TitleGroup("Jump Settings")] [Header("Jump Settings")]
        public float jumpForce;
        [SerializeField] private float maxJumpHeight   = 2f;   // Maximum height in meters/units
        [SerializeField] private float minJumpHeight   = 0.5f; // Minimum height when button is tapped
        [SerializeField] private float timeToApex      = 0.4f; // Time to reach the peak of the jump
        public                   float gravity         = -9.81f;
        public                   float groundedGravity = -0.5f;
        public                   float maxFallSpeed    = -20f;
        public                   float minJumpTime     = 0.2f;
        public                   float maxJumpTime     = 1f;
        [SerializeField] private float jumpCooldown    = 0.5f;
        [SerializeField] private float smoothTime      = 0.1f;
        [SerializeField] private float _coyoteTime     = 0.1f;
        [SerializeField] private float _jumpBufferTime = 0.1f;
        [SerializeField] private float _initialJumpVelocityMultiplier = 1.2f;
        
        private float _coyoteTimer;
        private float _timeSinceLastJump;
        private float _jumpBufferTimer;
        private float _smoothVelocity;
        public  float lastJumpTime;
        private bool  _hasJumped = false;
        private bool  _isLeaping;
        
        [TitleGroup("Extras")]
        [Header("Ground Settings")]
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private float     minimumMoveSpeed = 0.1f;
        [SerializeField] public LayerMask groundMask;

        [Header("References")] 
        [SerializeField] private PlayerStatsHandler playerStatsHandler;
        [SerializeField] public CharacterController characterController;
        [SerializeField] private Transform    cameraTransform;
        [SerializeField] private GameObject cameraHolder;
        [SerializeField] private Animator    animator; // hand animator -- for walking with weapon
        
        [TitleGroup("Toggles")]
        [Header("Headbob")] 
        [SerializeField] private bool  headbobEnabled;
        
        [TitleGroup("extra extras")]
        public Transform CameraTransform => cameraTransform;
        public bool  HeadbobEnabled   => headbobEnabled;
        public Vector3 CurrentMoveVelocity => _currentMoveVelocity;
        public float CrouchHeight => crouchHeight;
        public float CrouchSpeedMultiplier => crouchSpeedMultiplier;

        public Vector3 PlayerPosition => transform.position;
        
        public bool IsLeaping => _isLeaping;
        
        public InputManager inputManager;
        private bool isCrouching;
        
        public Vector3   moveVelocity;
        public Vector3 verticalVelocity;
        private Vector2   input;
        private bool     isLocked;
        
        private Action    _crouchAction;
        
        private Coroutine crouchCoroutine;
        private Vector3   initialCameraHolderPosition;


        public bool IsBlocking { get; private set; }

        public void SetBlocking(bool isBlocking)
        {
            IsBlocking = isBlocking;
        }

        private void Update()
        {
            input = InputManager.State.MoveDirection.normalized;
            if(InputManager.State.IsJumping) _jumpBufferTimer = _jumpBufferTime;
            _jumpBufferTimer -= Time.deltaTime;
            
            if (!IsGrounded(out _))
            {
                _coyoteTimer -= Time.deltaTime;
            }
            UpdateAnimatorMovement();
        }

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            if (!IsBlocking)
            {
                playerStatsHandler.TakeDamage(damage);
            }
            else
            {
                
            }
        //throw new NotImplementedException();
        }

        public void ForceState(PlayerState state)
        {
            TransitionToState(state);
        }
        
        #region Setup

        void Awake()
        {
            States[PlayerState.Idle]    = new PlayerIdleState
                (PlayerState.Idle, this);
            
            States[PlayerState.Walking] = new PlayerWalkingState
                (PlayerState.Walking, this);
            
            States[PlayerState.Falling] = new PlayerFallingState
                (PlayerState.Falling, this);
            
            States[PlayerState.Jumping] = new PlayerJumpingState
                (PlayerState.Jumping, this);
            
            States[PlayerState.Locked]  = new PlayerLockedState
                (PlayerState.Locked, this);
            States[PlayerState.Crouching]  = new PlayerCrouchingState
                (PlayerState.Crouching, this);
            States[PlayerState.Leaping] = new PlayerLeapingState
                (PlayerState.Leaping, this);
                
            
            CurrentState                =  States[PlayerController.PlayerState.Idle];
            _targetSpeedMultiplier      =  1f;
            _standingHeight             =  characterController.height;
            _coyoteTimer                =  _coyoteTime;
            _jumpBufferTimer            =  _jumpBufferTime;
            _timeSinceLastJump          =  0;
            _hasJumped                  =  false;
            _crouchAction               =  () => SetCrouching(!isCrouching);
            InputManager.Crouch  += _crouchAction;
            initialCameraHolderPosition =  cameraHolder.transform.localPosition;

            
            CalculateJumpParameters();
        }
        
        private void CalculateJumpParameters()
        {
            // Using physics equations:
            // h = v0*t + (1/2)*a*t^2
            // At apex: v = v0 + a*t = 0
            // Therefore: v0 = -a*t
        
            // Calculate gravity needed for desired apex time
            //gravity = (-2f * maxJumpHeight) / (timeToApex * timeToApex);
        
            // Calculate initial jump velocity needed
            //jumpForce = -gravity * timeToApex;
        
            // Calculate minimum jump velocity (for tap jumps)
            float minJumpVelocity = Mathf.Sqrt(-2f * gravity * minJumpHeight);
            //maxFallSpeed = -jumpForce; // Terminal velocity = max jump speed
        }
        private void OnDestroy()
        {
            InputManager.Crouch -= _crouchAction;

        }

        #endregion
        
        #region Grounded Checks

        public bool IsGrounded(out Vector3 groundNormal)
        {
            groundNormal = Vector3.up;
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance * 1.5f, groundMask))
            {
                groundNormal = hit.normal;
                _coyoteTimer = _coyoteTime;
                return true;
            }
            _coyoteTimer -= Time.deltaTime;

            return _coyoteTimer > 0 || characterController.isGrounded;
        }

        public bool IsGrounded()
        {
            return IsGrounded(out _);
        }
        public bool HasMovementInput()
        {
            return input.magnitude > minimumMoveSpeed;
        }

        #endregion
        
        #region Movement
        public void Move(float speed =1f)
        {
            ApplyHorizontalMovement(speed);
            ApplyVerticalMovement();
            ApplyFinalMovement();
        }

        private void ApplyHorizontalMovement(float speed = 1f)
        {
            Vector3 moveDirection = new Vector3(input.x, 0, input.y);
            if (moveDirection != Vector3.zero)
            {
                moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * moveDirection;
                if (_currentMoveVelocity.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            _targetMoveVelocity = GetMovementDirection(out _) * (moveSpeed * speed * _targetSpeedMultiplier);
        }
        
  private void ApplyVerticalMovement()
        {
            if (IsGrounded())
            {
                verticalVelocity.y = Mathf.SmoothDamp(verticalVelocity.y, groundedGravity, ref _smoothVelocity, smoothTime);
            }
            else
            {
                verticalVelocity.y = Mathf.Max(
                    verticalVelocity.y + gravity * Time.deltaTime,
                    maxFallSpeed
                );
            }
        }

        private void ApplyFinalMovement()
        {
            float currentAcceleration = HasMovementInput() ? acceleration : deceleration;
            Vector3 velocityChange = (_targetMoveVelocity - _currentMoveVelocity).normalized * 
                                     currentAcceleration * Time.deltaTime;
            _currentMoveVelocity = Vector3.MoveTowards(_currentMoveVelocity, _targetMoveVelocity,
                currentAcceleration * Time.deltaTime);
            if (velocityChange.magnitude > (_targetMoveVelocity - _currentMoveVelocity).magnitude)
            {
                _currentMoveVelocity = _targetMoveVelocity;
            }
            else
            {
                _currentMoveVelocity += velocityChange;
            }
            Vector3 finalVelocity = _currentMoveVelocity + verticalVelocity;
            characterController.Move(finalVelocity * Time.deltaTime);
        }
        
        private Vector3 GetMovementDirection(out Vector3 groundNormal)
        {
            groundNormal = Vector3.up;
            Vector3 moveDirection = new Vector3(input.x, 0, input.y);
            if (moveDirection != Vector3.zero)
            {
                moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * moveDirection;
                if (IsGrounded(out groundNormal))
                    moveDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal);

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            return moveDirection;
        }
        /*
        public bool IsFacingObstacle()
        {
            Vector3 rayStart = transform.position + Vector3.up * 0.5f; // Start at waist height
            Vector3 rayDirection = transform.forward;
            float rayDistance = 0.7f; // Check half meter ahead
            
            // Check if there's an obstacle in front
            if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, rayDistance))
            {
                return true;
            }
            
            return false;
        }
        */
        public bool IsFacingObstacle()
        {
            Vector3 center      = characterController.transform.position;
            float   radius      = characterController.radius * 0.8f;
            float   rayDistance = characterController.radius + 0.5f;

            // Get camera forward but ignore vertical component
            Vector3 checkDirection = cameraTransform.forward;
            checkDirection.y = 0;
            checkDirection.Normalize();

            // Primary sphere check at center mass
            if (Physics.SphereCast(center, radius, checkDirection,
                    out RaycastHit sphereHit, rayDistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                return true;
            }

            // Secondary checks at multiple heights
            Vector3[] rayStarts = new Vector3[]
            {
                center + (Vector3.up * characterController.height * 0.5f),  // Head level
                center,                                                     // Center mass
                center - (Vector3.up * characterController.height * 0.25f), // Knee level
                center - (Vector3.up * characterController.height * 0.4f)   // Foot level
            };

            foreach (Vector3 rayStart in rayStarts)
            {
                if (Physics.Raycast(rayStart, checkDirection,
                        out RaycastHit rayHit, rayDistance, groundMask, QueryTriggerInteraction.Ignore))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (!characterController || !cameraTransform) return;

            Vector3 center      = characterController.transform.position;
            float   radius      = characterController.radius * 0.8f;
            float   rayDistance = characterController.radius + 0.5f;

            // Get camera forward but ignore vertical component
            Vector3 checkDirection = cameraTransform.forward;
            checkDirection.y = 0;
            checkDirection.Normalize();

            // Draw spherecast
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center, radius);
            Gizmos.DrawWireSphere(center   + checkDirection * rayDistance, radius);
            Gizmos.DrawLine(center, center + checkDirection * rayDistance);

            // Draw raycasts
            Vector3[] rayStarts = new Vector3[]
            {
                center + (Vector3.up * characterController.height * 0.5f),  // Head level
                center,                                                     // Center mass
                center - (Vector3.up * characterController.height * 0.25f), // Knee level
                center - (Vector3.up * characterController.height * 0.4f)   // Foot level
            };

            Gizmos.color = Color.green;
            foreach (Vector3 rayStart in rayStarts)
            {
                Gizmos.DrawLine(rayStart, rayStart + checkDirection * rayDistance);
                Gizmos.DrawWireSphere(rayStart, 0.05f);
            }
        }

        #endregion

        #region physics
        public void ApplyGravity(float gravityValue)
        {
            verticalVelocity.y += gravityValue * Time.deltaTime;
        }

        public void ResetVelocity()
        {
            ResetVerticalVelocity();
            ResetHorizontalVelocity();
        }
        public void ResetVerticalVelocity()
        {
            verticalVelocity = Vector3.zero;
        }
        public void ResetHorizontalVelocity()
        {
            _currentMoveVelocity = Vector3.zero;
        }

        #endregion
        
        #region Jump

        public void Jump()
        {
            verticalVelocity.y = jumpForce *_initialJumpVelocityMultiplier;
            lastJumpTime       = Time.time;
            _timeSinceLastJump = 0;
        }

        public bool CheckJump()
        {
            //var condition = (Time.time > lastJumpTime + jumpCooldown)
            //                 && IsGrounded();
            if(_jumpBufferTimer > 0 && _coyoteTimer > 0 && !_hasJumped)
            {
                _hasJumped       = true;
                _jumpBufferTimer = 0;
                _coyoteTimer     = 0;
                return true;
            }
            return false;
        }
        public void ResetJump()
        {
            _hasJumped = false;
        }


        #endregion

        #region Leaping

        public void SetLeaping(bool leaping)
        {
            _isLeaping = leaping;
        }

        #endregion

        #region Crouch

        public bool Crouch()
{
    if (crouchCoroutine != null)
    {
        StopCoroutine(crouchCoroutine);
    }
    crouchCoroutine = StartCoroutine(ChangeHeight(crouchHeight));
    return true;
}

public bool UnCrouch()
{
    if (crouchCoroutine != null)
    {
        StopCoroutine(crouchCoroutine);
    }
    crouchCoroutine = StartCoroutine(ChangeHeight(_standingHeight));
    return true;
}

// Modify your SetCrouching method to handle failed attempts
private void SetCrouching(bool boolean)
{
    if (isCrouching == boolean) return;
    
    isCrouching = boolean;
    if (isCrouching)
    {
        if (!Crouch())
        {
            isCrouching = false;
        }
    }
    else
    {
        if (!UnCrouch())
        {
            isCrouching = true;
        }
    }
}

        public bool CheckCrouch()
        {
            return isCrouching;
        }
        
        private IEnumerator ChangeHeight(float targetHeight)
        {
            //TODO:: fix this garbage
            float   initialHeight = characterController.height;
            Vector3 initialCenter = characterController.center;
            Vector3 targetCenter  = new Vector3(0, targetHeight / 2, 0);
            float   duration      = 0.2f; // Duration of the transition
            float   elapsed       = 0f;
            Vector3 targetCameraHolderPosition = initialCameraHolderPosition + new Vector3(0, targetHeight - _standingHeight, 0);

            while (elapsed < duration)
            {
                elapsed                    += Time.deltaTime;
                characterController.height =  Mathf.Lerp(initialHeight, targetHeight, elapsed   / duration);
                characterController.center =  Vector3.Lerp(initialCenter, targetCenter, elapsed / duration);
                cameraHolder.transform.localPosition = Vector3.Lerp(initialCameraHolderPosition, targetCameraHolderPosition, elapsed / duration);

                yield return null;
            }

            characterController.height           = targetHeight;
            characterController.center           = targetCenter;
            cameraHolder.transform.localPosition = targetCameraHolderPosition;
        }
        #endregion
        
        #region Player Lock

        public void LockPlayer()
        {
            isLocked         = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
            input            = Vector2.zero;
        }
    
        public void UnlockPlayer()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
            isLocked         = false;
        }

        #endregion
        
        #region Animation

        private void UpdateAnimatorMovement()
        {
            // Pass the normalized movement speed (magnitude) to the animator as a "Speed" float parameter
            //if (animator == null) return;
            //animator.SetFloat("Speed", _currentMoveVelocity.magnitude/moveSpeed);
        }

        #endregion


    }
}