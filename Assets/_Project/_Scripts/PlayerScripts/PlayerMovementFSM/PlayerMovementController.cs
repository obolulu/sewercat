using UnityEngine;

namespace _Project._Scripts.PlayerScripts
{
    public class PlayerController : StateManager<PlayerController.PlayerState>
    {
        public enum PlayerState
        {
            Idle,
            Walking,
            Falling,
            Jumping,
            Locked
        }
        public PlayerState currentPlayerState;
        
        [Header("Movement Settings")]
        public float moveSpeed = 6f;
        public float rotationSpeed   = 15f;

        
        [Header("Jump Settings")]
        public float jumpForce       = 5f;
        public float gravity         = -9.81f;
        public float groundedGravity = -0.5f;
        public float maxFallSpeed    = -20f;
        public float minJumpTime = 0.2f;
        public float maxJumpTime     = 1f;
        
        
        [Header("Ground Settings")]
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private float     minimumMoveSpeed = 0.1f;
        [SerializeField] private LayerMask groundMask;
        
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform    cameraTransform;
        public InputManager inputManager;

        [Header("Tilt Settings")] 
        [SerializeField] private float tiltAngle = 10f;
        [SerializeField] private float tiltSpeed = 5f;

        
        public Vector3   moveVelocity;
        public Vector3 verticalVelocity;
        private Vector2   input;
        private bool     isLocked;
        
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
            
            CurrentState                                   = States[PlayerController.PlayerState.Idle];
        }
        private void Update()
        {
            input = InputManager.moveDirection;
        }
    
        public bool IsGrounded()
        {
            return Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, 0.5f, groundMask);
            return characterController.isGrounded;
        }
    
        public void Move()
        {
            ApplyHorizontalMovement();
            ApplyVerticalMovement();
            ApplyFinalMovement();
            ApplyCameraTilt();
        }
        private void ApplyCameraTilt()
        {
            float targetTilt = input.x * tiltAngle;
            float currentTilt = Mathf.LerpAngle(cameraTransform.localEulerAngles.z, targetTilt, tiltSpeed * Time.deltaTime);
            cameraTransform.localEulerAngles = new Vector3(cameraTransform.localEulerAngles.x, cameraTransform.localEulerAngles.y, currentTilt);
        }
        private void ApplyHorizontalMovement()
        {
            Vector3 moveDirection = new Vector3(input.x, 0, input.y);
            if (moveDirection != Vector3.zero)
            {
                moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * moveDirection;
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            moveVelocity = moveDirection * moveSpeed;
        }
        
        private void ApplyVerticalMovement()
        {
            if (IsGrounded() && verticalVelocity.y < 0)
            {
                verticalVelocity.y = groundedGravity;
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
            Vector3 finalVelocity = moveVelocity + verticalVelocity;
            characterController.Move(finalVelocity * Time.deltaTime);
        }

    
        public void ApplyGravity(float gravityValue)
        {
            verticalVelocity.y += gravityValue * Time.deltaTime;
            characterController.Move(verticalVelocity * Time.deltaTime);
        }
        
        public bool HasMovementInput()
        {
            return input.magnitude > minimumMoveSpeed;
        }

        public void Jump()
        {
            verticalVelocity.y = jumpForce;
        }
    
        public void ResetVerticalVelocity()
        {
            verticalVelocity.y = 0f;
        }
        public void LockPlayer()
        {
            isLocked = true;
            input    = Vector2.zero;
        }
    
        public void UnlockPlayer()
        {
            isLocked = false;
        }
    }
}