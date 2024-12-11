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
        public float jumpForce       = 5f;
        public float gravity         = -9.81f;
        public float groundedGravity = -0.5f;
        
        [Header("Ground Settings")]
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private float     minimumMoveSpeed = 0.1f;
        [SerializeField] private LayerMask groundMask;
        
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform    cameraTransform;
        public InputManager inputManager;
        
        public Vector3   velocity;
        private Vector2   input;
        private bool     isLocked;
        
        
        void Awake()
        {
            States[PlayerController.PlayerState.Idle]    = new PlayerIdleState
                (PlayerController.PlayerState.Idle, this);
            
            States[PlayerController.PlayerState.Walking] = new PlayerWalkingState
                (PlayerController.PlayerState.Walking, this);
            
            States[PlayerController.PlayerState.Falling] = new PlayerFallingState
                (PlayerController.PlayerState.Falling, this);
            
            States[PlayerController.PlayerState.Jumping] = new PlayerJumpingState
                (PlayerController.PlayerState.Jumping, this);
            
            States[PlayerController.PlayerState.Locked]  = new PlayerLockedState
                (PlayerController.PlayerState.Locked, this);
            
            CurrentState                                   = States[PlayerController.PlayerState.Idle];
        }
        private void Update()
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input = input.normalized;
        }
    
        public bool IsGrounded()
        {
            return Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, 0.5f, groundMask);
            return characterController.isGrounded;
        }
    
        public void Move()
        {
            Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        
            if (moveDirection != Vector3.zero)
            {
                moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * moveDirection;
            
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        
            characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));
        }
    
        public void ApplyGravity(float gravityValue)
        {
            velocity.y += gravityValue * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        
        public bool HasMovementInput()
        {
            return input.magnitude > minimumMoveSpeed;
        }

        public void Jump()
        {
            velocity.y = jumpForce;
        }
    
        public void ResetVerticalVelocity()
        {
            velocity.y = 0f;
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